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

namespace Project_VW
{
    /// <summary>
    /// Lógica de interacción para ResetPassword.xaml
    /// </summary>
    public partial class ResetPassword : UserControl
    {
        DB db;
        List<ComboBoxPairs> cbp;

        public ResetPassword()
        {
            db = new DB();
            InitializeComponent();
            fillUsers();
        }

        public void fillUsers()
        {
            cbp = new List<ComboBoxPairs>();
            string qry_getUsers = "SELECT ID, user FROM usuarios;";
            db.openConn();
            using (db.setComm(qry_getUsers))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    cbp.Add(new ComboBoxPairs(
                      Convert.ToString(db.getReader()["user"]),
                      Convert.ToString(db.getReader()["ID"])
                    ));
                }
            }
            comboUsuarios.DisplayMemberPath = "user";
            comboUsuarios.SelectedValuePath = "ID";
            comboUsuarios.ItemsSource = cbp;
            db.closeConn();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            if (comboUsuarios.SelectedValue == null)
            {
                MessageBox.Show("Selecciona Un usuario y escribe una contraseña.");
                return;
            }
            if(String.IsNullOrEmpty(newPassword.Text))
            {
                MessageBox.Show("Escribe al menos un caracter.");
                return;
            }
            ComboBoxPairs cbp = (ComboBoxPairs)comboUsuarios.SelectedItem;
            string user_selected = cbp.user;
            int ID_user = Convert.ToInt32(cbp.ID);
            string newPass = newPassword.Text;
            string qry_chngPass = "UPDATE usuarios SET password = '" + newPass + "' WHERE ID = " + ID_user;
            db.openConn();
            using (db.setComm(qry_chngPass))
            {
                db.getComm().ExecuteNonQuery();
            }
            db.closeConn();
            MessageBox.Show("Contraseña cambiada correctamente, recuerda proporcionar la contraseña: " + newPass);
            newPassword.Text = "";
        }
    }
}
