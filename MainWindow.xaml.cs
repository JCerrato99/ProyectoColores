using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
// Para poder usar config y PropertyInfo
using System.Configuration;
using System.Reflection;
// Para la lectura-escritura de .config
using System.IO;

namespace ProyectoColores
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string contenido_config;


        public MainWindow()
        {
            InitializeComponent();
            contenido_config = LeerArchivo(@"config.txt");
            if (contenido_config == "")
            {
                contenido_config = "Arial";
            }

            // Usa los valores de App.config
            string fuentePrincipal = ConfigurationManager.AppSettings["FuentePrincipal"];

            // Establece la fuente principal
            txt2.FontFamily = new FontFamily(fuentePrincipal);

            // Carga datos al ComboBox de las fuentes de texto disponibles
            Ffuente.ItemsSource = Fonts.SystemFontFamilies;
        }


        private void Ffuente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener la fuente de texto seleccionada
            FontFamily selectedFont = Ffuente.SelectedItem as FontFamily;

            // Establecer la fuente de texto del TextBox txt2
            txt2.FontFamily = selectedFont;

            // Actualizar la configuración en el archivo .config
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["FuentePrincipal"].Value = selectedFont.Source;
            config.Save();
        }

        private void BConf1_Click(object sender, RoutedEventArgs e)
        {
            // Verificar la fuente actual y cambiarla
            if (txt1.FontFamily.Source == "Arial")
            {
                btn1.Content = "Cambiar a Verdana";
                txt1.FontFamily = new FontFamily("Verdana");
            }
            else
            {
                btn1.Content = "Cambiar a Arial";
                txt1.FontFamily = new FontFamily("Arial");
            }

            // Actualizar la configuración en el archivo .config
            EscribirEnArchivo(txt1.FontFamily.Source, @"config.txt");
        }

        public void EscribirEnArchivo(string texto, string rutaArchivo)
        {
            // Comprobamos si el archivo ya existe
            if (!File.Exists(rutaArchivo))
            {
                // Si no existe, lo creamos
                using (FileStream fs = File.Create(rutaArchivo)) { }
            }

            // Sobreescribimos el contenido del archivo con el nuevo texto
            File.WriteAllText(rutaArchivo, texto);
        }

        public string LeerArchivo(string rutaArchivo)
        {
            // Comprobamos si el archivo existe
            if (!File.Exists(rutaArchivo))
            {
                // Creamos el archivo si no existe
                File.Create(rutaArchivo).Close();
            }

            // Leemos el contenido del archivo y lo devolvemos como un string
            return File.ReadAllText(rutaArchivo);
        }
    }
}
