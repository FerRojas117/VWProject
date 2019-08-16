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
            conexion = new SQLiteConnection("Data Source=PruebasNar_DB.db;Version=3;");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            user = usuario.Text;
            pass = passwordBox.Password.ToString();
            inicioSesion(user, pass);
        }
        public void inicioSesion(string user, string pass)
        {
            // check if user exists
            string user_exists = "";
            openConn();
            string qry_know_userExists = "SELECT count(user) AS numero FROM usuarios WHERE user = '" + user + "'";
            using (command = new SQLiteCommand(qry_know_userExists, conexion) )
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while ( reader.Read() )
                {
                    user_exists = Convert.ToString(reader["numero"]);
                }
            }
            // if not, then will return
            if( user_exists != "1" )
            {
                sendMBandCloseConn("Revisa tu usuario, no es correcto el nombre.");
                return;
            }
            // else will check password
            string qry_get_userID = "SELECT * FROM usuarios WHERE user ='" + user + "'";
            string user_ID = "", user_name = "", tipo_user = "", activo = "", user_password  = "";

            using (command = new SQLiteCommand(qry_get_userID, conexion))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while ( reader.Read() )
                {
                    user_ID = Convert.ToString(reader["ID"]);
                    user_name = Convert.ToString(reader["user"]);
                    user_password = Convert.ToString(reader["password"]);
                    tipo_user = Convert.ToString(reader["tipo_user"]);
                    activo = Convert.ToString(reader["activo"]);
                }
                // here we will check if user is active(is not deleted)
                if ( activo == "2" )
                {
                    sendMBandCloseConn("Lo siento. No tienes permiso para entrar al sistema");
                    return;
                }
                // here we will check the password
                if ( pass != user_password )
                {
                    sendMBandCloseConn("Revisa tu contraseña, no es correcta.");
                    return;
                }
                // else, we will store data session and Update last login
                string qry_updt_lastLogin = "UPDATE usuarios SET last_login = datetime() WHERE ID ='" + user_ID + "'";
                using (command = new SQLiteCommand(qry_updt_lastLogin, conexion))
                {

                }
                SesionUsuario.setID(Convert.ToInt32(user_ID));
                SesionUsuario.setUser(user_name);
                SesionUsuario.setUserTipo(Convert.ToInt32(tipo_user));

                MessageBox.Show(
                    "Inicio de Sesión correcto. " +
                    "UID: " + SesionUsuario.getUserID() +
                    ", UN: " + SesionUsuario.getUser() +
                    ", UT: " + SesionUsuario.getUserTipo()
                );
            }
            closeConn();

            // pasar a siguiente página

            admin Admin = new admin();
            NavigationService.Navigate(Admin);
;        }

        public void openConn()
        {
            try
            {
                conexion.Open();
            }
            catch (Exception ex)
            {
               sendMBandCloseConn("No pudo abrirse de forma correcta la base de datos.\n" + ex.Message);
            }
        }

        public void closeConn()
        {
            try
            {
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No pudo cerrarse de forma correccta la base de datos.\n" +
                    ex.Message
                );
            }
        }

        public void sendMBandCloseConn(string message)
        {
            MessageBox.Show(message);
            closeConn();
        }
    }
}
