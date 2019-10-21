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
        List<Rutas> routes;
        public AplicacionesExternas()
        {
            db = new DB();
            routes = new List<Rutas>();
            InitializeComponent();
            getOtherRoutes();
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
                case "card_KPM":
                    ID = 3;
                    openLink();
                    break;
                case "card_PP":
                    ID = 4;
                    openLink();
                    break;
                case "card_Vacaciones":
                    ID = 5;
                    openLink();
                    break;
                case "card_JI":
                    ID = 6;
                    openLink();
                    break;
                case "card_SRH":
                    ID = 7;
                    openLink();
                    break;
                case "card_Wiki":
                    ID = 8;
                    openLink();
                    break;
                case "card_FB":
                    ID = 9;
                    openLink();
                    break;
                case "card_ODIS":
                    ID = 10;
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
                case "edit_KPM":
                    ID = 3;
                    EditLink();
                    break;
                case "edit_PP":
                    ID = 4;
                    EditLink();
                    break;
                case "edit_Vacaciones":
                    ID = 5;
                    EditLink();
                    break;
                case "edit_JI":
                    ID = 6;
                    EditLink();
                    break;
                case "edit_SRH":
                    ID = 7;
                    EditLink();
                    break;
                case "edit_Wiki":
                    ID = 8;
                    EditLink();
                    break;
                case "edit_FB":
                    ID = 9;
                    EditLink();
                    break;
                case "edit_ODIS":
                    ID = 10;
                    EditLink();
                    break;
                default:
                    break;
            }
        }

        private void itemCLick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("No se pudo abrir el recurso");
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
            try
            {
                System.Diagnostics.Process.Start(ruta);
            } catch(Exception e)
            {
                MessageBox.Show("No es posible abrir la ruta: " + ruta);
                MessageBox.Show(e.Message);
            }
                     
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

        public void getOtherRoutes()
        {
            string qry_getAllRuta = "SELECT ruta, nombre FROM rutas WHERE ID > 10";
            db.openConn();
            using (db.setComm(qry_getAllRuta))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    routes.Add(new Rutas{
                        ruta = Convert.ToString(db.getReader()["ruta"]),
                        nombre = Convert.ToString(db.getReader()["nombre"])
                    }); 
                }
            }
            db.closeConn();
            OtherLinks.DataContext = routes;
        }
    }

    public class Rutas
    {
        public string nombre { get; set; }
        public string ruta { get; set; }
    }
}
