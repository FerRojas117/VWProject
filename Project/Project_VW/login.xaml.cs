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
using MaterialDesignThemes.Wpf;
using System.ComponentModel;

namespace Project_VW
{
    /// <summary>
    /// Lógica de interacción para login.xaml
    /// </summary>
    public partial class login : Page
    {
        DB db;
        string pass;
        string user;
        private bool _IsDialogOpen;
        string user_ID = "", user_name = "", tipo_user = "", activo = "", user_password = "";
        string isFirstLogin = "";

        public bool IsDialogOpen
        {
            get;
            set;
        }
        
        public login()
        {
            InitializeComponent();
            db = new DB();
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
            db.openConn();
            string qry_know_userExists = "SELECT count(user) AS numero FROM usuarios WHERE user = '" + user + "'";

            using (db.setComm(qry_know_userExists))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    user_exists = Convert.ToString(db.getReader()["numero"]);
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

            using (db.setComm(qry_get_userID))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    user_ID = Convert.ToString(db.getReader()["ID"]);
                    user_name = Convert.ToString(db.getReader()["user"]);
                    user_password = Convert.ToString(db.getReader()["password"]);
                    tipo_user = Convert.ToString(db.getReader()["tipo_user"]);
                    activo = Convert.ToString(db.getReader()["activo"]);
                    isFirstLogin = Convert.ToString(db.getReader()["isFirstLogin"]);
                }

                if (activo == "2")
                {
                    sendMBandCloseConn("Lo siento. No tienes permiso para entrar al sistema");
                    return;
                }
                // here we will check the password
                if (pass != user_password)
                {
                    sendMBandCloseConn("Revisa tu contraseña, no es correcta.");
                    return;
                }

                // We Verify if is firstLogin of user, if it is, 
                // then we should ask the user to change password and return

            }


            
            db.closeConn();

            if (isFirstLogin == "1")
            {
                dialogi.IsOpen = true;
            }
            else
            {
                // else, we will store data session and Update last login

              
                SesionUsuario.setID(Convert.ToInt32(user_ID));
                SesionUsuario.setUser(user_name);
                SesionUsuario.setUserTipo(Convert.ToInt32(tipo_user));

                MessageBox.Show(
                    "Inicio de Sesión correcto. " +
                    "UID: " + SesionUsuario.getUserID() +
                    ", UN: " + SesionUsuario.getUser() +
                    ", UT: " + SesionUsuario.getUserTipo()
                );

                // pasar a siguiente página

                admin Admin = new admin();
                NavigationService.Navigate(Admin);
            }

        }

        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            string qry_updt_lastLogin = "UPDATE usuarios SET password = '" + PasswordTBox.Text + "' WHERE ID ='" + user_ID + "'";
            db.openConn();
            using (db.setComm(qry_updt_lastLogin))
            {
                db.getComm().ExecuteNonQuery();
            }
            db.closeConn();
            passChanged.IsOpen = true;

        }

        private void passwordChange_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;
        }


       

        public void sendMBandCloseConn(string message)
        {
            MessageBox.Show(message);
            db.closeConn();
        }
    }

    public class MainWindowsViewModel : ViewModelBase
    {
        
    }
}
