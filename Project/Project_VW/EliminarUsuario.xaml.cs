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
        Dictionary<string, string> usuarios;
        DB db;
        public EliminarUsuario()
        {
            db = new DB();
            usuarios = new Dictionary<string, string>();
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
                    usuarios.Add(
                      Convert.ToString(db.getReader()["ID"]),
                      Convert.ToString(db.getReader()["user"])
                    );
               }
            }
            db.closeConn();
        }

        public Dictionary<string, string> Usuarios
        {
            get
            {
                return usuarios;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // 
        }
    }
}
