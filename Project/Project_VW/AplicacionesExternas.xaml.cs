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
using MaterialDesignThemes.Wpf;
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
        int ID = 0;
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
                    ID = 1;
                    openLink();
                    break;
                case "card_SI":
                    ID = 2;
                    openLink();
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
                    ID = 1;
                    EditLink();
                    break;
                case "edit_SI":
                    ID = 2;
                    EditLink();
                    break;
                default:
                    break;
            }
        }

        public void EditLink()
        {
            getRuta();
            NuevaRuta.Text = ruta;
            dialogi.IsOpen = true;         
        }

        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            string qry_updateRuta = "UPDATE rutas SET ruta = '" + NuevaRuta.Text + "' WHERE ID = " + ID; ;
            db.openConn();
            using (db.setComm(qry_updateRuta))
            {
                db.getComm().ExecuteNonQuery();
            }
            db.closeConn();            
            RouteChanged.IsOpen = true;
        }

        private void RouteChange_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;
        }

        public void openLink()
        {
            getRuta();
            //// Navigate to
            System.Diagnostics.Process.Start(ruta);
            
        }

        public void getRuta()
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
            db.closeConn();
        }
    }
}
