﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace Project_VW
{
    /// <summary>
    /// Lógica de interacción para NuevoUsuario.xaml
    /// </summary>
    public partial class NuevoUsuario : UserControl
    {
        DB db;
        string nuevoUsuario = "";
        string password = "";
        string confirmPassword = "";
        string tipo_usuario = "2";
        int affectedRows = 0;
        string userExist = "SELECT COUNT(ID) AS userExist FROM usuarios WHERE user = ";
        int registerCounter;
        public NuevoUsuario()
        {
            db = new DB();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            registerCounter = 0;
            nuevoUsuario = usuario_box.Text;           

            db.openConn();
            // count if user exist already
            using (db.setComm(userExist + "'"+ nuevoUsuario+ "'"))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    registerCounter = Convert.ToInt32(db.getReader()["userExist"]);
                }
            }
            // there is an user, so we close DB and return  
            if (registerCounter >= 1)
            {
                db.sendMBandCloseConn("Este nombre de Usuario: " + nuevoUsuario + ", ya existe. Elige otro por favor.");
                return;
            }

            db.closeConn();

            password = password_box.Password.ToString();
            confirmPassword = confirmPassword_box.Password.ToString();
            if( password != confirmPassword )
            {
                MessageBox.Show("Las contraseñas no coinciden, revísalas por favor.");
                return;
            }
            
            if (esAdmin.IsChecked == true)
                tipo_usuario = "1";
            else tipo_usuario = "2";

            db.openConn();
            string qry_new_user = "INSERT INTO usuarios (password, user, tipo_user, activo, last_login, isFirstLogin)";
            qry_new_user += "VALUES('" + password + "', '" + nuevoUsuario + "', '" +  tipo_usuario + "', 1, 'fecha', " + 1 +")";
            using (db.setComm(qry_new_user))
            {
                affectedRows = db.getComm().ExecuteNonQuery();
            }

            if (affectedRows == 0)
            {
                db.sendMBandCloseConn("No se pudo añadir Usuario. Inténtalo de nuevo");
                return;
            }

            db.sendMBandCloseConn("Usuario añadido exitosamente.");

            usuario_box.Clear();
            password_box.Clear();
            confirmPassword_box.Clear();
        }
    }
}
