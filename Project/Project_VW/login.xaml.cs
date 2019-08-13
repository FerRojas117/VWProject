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
using System.Data.SQLite;

namespace Project_VW
{
    /// <summary>
    /// Lógica de interacción para login.xaml
    /// </summary>
    public partial class login : Page
    {
        SQLiteConnection conexion;
        SQLiteCommand command;
        string pass;
        string user;
        public login()
        {
            InitializeComponent();
        }

        // Creates a connection with our database file.
        void connectToDatabase()
        {
            conexion = new SQLiteConnection("Data Source=PruebasNar_DB.db;Version=3;");
            conexion.Open();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            user = usuario.Text;
            pass = password.Text;
            inicioSesion(user, pass);
        }
        public void inicioSesion(string user, string pass)
        {
            string theQuery = "SELECT * FROM usuarios WHERE ";
        }
    }
}
