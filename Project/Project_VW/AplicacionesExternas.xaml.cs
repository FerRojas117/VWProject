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
using System.Data.SQLite;

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
            
            if (SesionUsuario.getUserTipo() == 2)
            {
                edit_Intranet.Visibility = Visibility.Collapsed;
                edit_SI.Visibility = Visibility.Collapsed;
                edit_KPM.Visibility = Visibility.Collapsed;
                edit_PP.Visibility = Visibility.Collapsed;
                edit_Vacaciones.Visibility = Visibility.Collapsed;
                edit_JI.Visibility = Visibility.Collapsed;
                edit_SRH.Visibility = Visibility.Collapsed;
                edit_Wiki.Visibility = Visibility.Collapsed;
                edit_FB.Visibility = Visibility.Collapsed;
                edit_ODIS.Visibility = Visibility.Collapsed;
            }
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

        public void itemClick(object sender, ExceptionRoutedEventArgs e)
        {
            
        }

        private void checkRutas(object sender, RoutedEventArgs e)
        {
            string qry_insertRuta = "INSERT INTO rutas (ruta, nombre) VALUES ('";

            try
            {

                db.openConn();
                db.tr = db.getConn().BeginTransaction();
                foreach (Rutas r in routes)
                {
                    if(String.IsNullOrEmpty(r.ID))
                    {
                        string newRoute = qry_insertRuta + r.ruta + "', '" + r.nombre + "')";
                        using (db.setComm(newRoute))
                        {
                            db.getComm().ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string updateRoute = "UPDATE rutas SET ";
                        updateRoute += "ruta = " + "'" + r.ruta + "', ";
                        updateRoute += "nombre = " + "'" + r.nombre + "' ";
                        updateRoute += "WHERE ID = " + r.ID;
                        using (db.setComm(updateRoute))
                        {
                            db.getComm().ExecuteNonQuery();
                        }
                    }
                }
                db.tr.Commit();
                MessageBox.Show("Rutas Actualizadas.");

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: {0}", ex.ToString());

                if (db.tr != null)
                {
                    try
                    {
                        db.tr.Rollback();
                    }
                    catch (SQLiteException ex2)
                    {

                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());

                    }
                    finally
                    {
                        db.tr.Dispose();
                    }
                }
            }
            finally
            {
                if (db.getComm() != null)
                {
                    db.getComm().Dispose();
                }

                if (db.tr != null)
                {
                    db.tr.Dispose();
                }

                if (db.getConn() != null)
                {
                    try
                    {
                        db.getConn().Close();

                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        db.getConn().Dispose();
                    }
                }
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
            string qry_getAllRuta = "SELECT ID, ruta, nombre FROM rutas WHERE ID > 10";
            db.openConn();
            using (db.setComm(qry_getAllRuta))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    routes.Add(new Rutas{
                        ID = Convert.ToString(db.getReader()["ID"]),
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
        public string ID { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
    }



}
