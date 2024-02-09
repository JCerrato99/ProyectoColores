using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                contenido_config = "Comic Sans MS";
            }


            // Usa los valores de App.config
            string backgroundSrt = ConfigurationManager.AppSettings["FuentePrincipal"];
            Color backgroundCColor = (Color)ColorConverter.ConvertFromString(backgroundSrt);

            string Adorno2srt = ConfigurationManager.AppSettings["Fuente2"];
            Color Adorno2color = (Color)ColorConverter.ConvertFromString(Adorno2srt);
            txt2.Background = new SolidColorBrush(Adorno2color);

            // Carga datos al ComboBox de los posibles colores del sistema.
            Ffuente.ItemsSource = typeof(Colors).GetProperties();
        }

        private void Ffuente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lee el valor seleccionado
            string colorselecionadoSrt = (Ffuente.SelectedItem as PropertyInfo).GetValue(Ffuente, null).ToString();
            txt2.Text = colorselecionadoSrt;
            // actualiza el color de fondo, guardando la selección
            Color colorselecionado = (Color)(Ffuente.SelectedItem as PropertyInfo).GetValue(Ffuente, null);
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["FuentePrincipal"].Value = colorselecionado + "";
            config.Save();
            // Carga ese valor, de lo contrario para ver los cambios tiene que ejecutarse otra vez.
        }

        private void BConf1_Click(object sender, RoutedEventArgs e)
        {
            //string contenido = LeerArchivo(@"config.txt");
            if (contenido_config == "Arial")
            {
                btn1.Content = "Activa Azul";
                contenido_config = "Verdana";
                Color color_config = (Color)ColorConverter.ConvertFromString(contenido_config);
                txt1.Background = new SolidColorBrush(color_config);
            }
            else
            {
                btn1.Content = "Desactiva Azul";
                contenido_config = "Arial";
                Color color_config = (Color)ColorConverter.ConvertFromString(contenido_config);
                txt1.Background = new SolidColorBrush(color_config);
            }
            // Actualizando el ficheor .config
            EscribirEnArchivo(contenido_config, @"config.txt");
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
