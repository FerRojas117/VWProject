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
    /// Lógica de interacción para AplicacionesExternas.xaml
    /// </summary>
    public partial class AplicacionesExternas : UserControl
    {
        DB db;
        string ruta = "";
        public AplicacionesExternas()
        {
            db = new DB();
            InitializeComponent();            
        }

        private void redirect(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "card_Intranet":
                    openLink(1);
                    break;
                case "card_SI":
                    openLink(2);
                    break;
                default:
                    break;
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "edit_Intranet":
                    EditLink(1);
                    break;
                case "edit_SI":
                    EditLink(2);
                    break;
                default:
                    break;
            }
        }

        public void EditLink(int ID)
        {
            string qry_updateRuta = "UPDATE rutas SET ruta = '" + "https://portalvwm.vwm.na.vwg/s" +"' WHERE ID = 1";
            db.openConn();
            using (db.setComm(qry_updateRuta))
            {
                db.getComm().ExecuteNonQuery();
            }          
            db.closeConn();
            MessageBox.Show("Ruta actualizada.");
        }

        public void openLink(int ID)
        {
            string qry_getRuta = "SELECT ruta FROM rutas WHERE ID = ";
            qry_getRuta += ID;
            db.openConn();
            using (db.setComm(qry_getRuta))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    ruta = Convert.ToString(db.getReader()["ruta"]);
                }
            }
            //// Navigate to
            System.Diagnostics.Process.Start(ruta);
            db.closeConn();
        }
    }
}
