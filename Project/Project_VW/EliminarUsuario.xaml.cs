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
            InitializeComponent();
            fillUsers();
        }

        public void fillUsers()
        {
            cbp = new List<ComboBoxPairs>();
            string qry_getUsers = "SELECT ID, user FROM usuarios WHERE tipo_user = 2;";
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
            MessageBoxResult result = MessageBox.Show(
                "Estás seguro de eliminar el usuario: " + user_selected + ", con ID: " + ID_user + "?",
                "Eliminar Usuario",
                MessageBoxButton.YesNoCancel
            );

            switch (result)
            {
                case MessageBoxResult.Yes:
                    int affectedRows = 0;
                    string qry_deleteUser = "DELETE FROM usuarios WHERE ID = '" + ID_user +"'";
                    db.openConn();
                    using (db.setComm(qry_deleteUser))
                    {
                        affectedRows = db.getComm().ExecuteNonQuery();
                    }
                    if (affectedRows == 0)
                    {
                        db.sendMBandCloseConn("Usuario No eliminado. Problema desconocido.");
                        return;
                    }
                    db.sendMBandCloseConn("Usuario Eliminado exitosamente.");
                    comboUsuarios.ItemsSource = null;   
                    fillUsers();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Usuario No eliminado.", "Eliminar Usuario");
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Usuario No eliminado.", "Eliminar Usuario");
                    break;
            }
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
