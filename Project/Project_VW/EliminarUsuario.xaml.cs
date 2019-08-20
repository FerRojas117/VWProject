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
    /// Lógica de interacción para EliminarUsuario.xaml
    /// </summary>
    public partial class EliminarUsuario : UserControl
    {

        List<ComboBoxPairs> cbp;
        DB db;
        public EliminarUsuario()
        {
            db = new DB();
            cbp = new List<ComboBoxPairs>();
            InitializeComponent();
            fillUsers();
        }

        public void fillUsers()
        {
            string qry_getUsers = "SELECT ID, user FROM usuarios";
            db.openConn();
            using (db.setComm(qry_getUsers))
            {
               db.setReader();
               while( db.getReader().Read() )
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxPairs cbp = (ComboBoxPairs)comboUsuarios.SelectedItem;
            string user_selected = cbp.user;
            string ID_user = cbp.ID;

            MessageBox.Show("Usuario seleccionado: " + user_selected + ", ID: " + ID_user);
        }
    }

    public class ComboBoxPairs
    {
        public string user { get; set; }
        public string ID { get; set; }

        public ComboBoxPairs(string user_p, string ID_P)
        {
            user = user_p;
            ID = ID_P;
        }
    }
}
