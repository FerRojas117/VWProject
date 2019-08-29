using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para admin.xaml
    /// </summary>
    public partial class admin : Page
    {
        DB db;
        int affectedRows = 0;
        
        public admin()
        {
            InitializeComponent();
            db = new DB();
            string qry_getEvento = "SELECT COUNT(nombre) AS numEventos FROM evento WHERE is_current = 1";
            string qry_getEventoNom = "SELECT nombre, ID FROM evento WHERE is_current = 1";
            string nom_evento_actual = "";
            int ID_evento_actual = 0;
            db.openConn();
            using (db.setComm(qry_getEvento))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    affectedRows = Convert.ToInt32(db.getReader()["numEventos"]);
                } 
            }
            if (affectedRows == 0)
            {
                nombreEvento.Text = SesionUsuario.getUserTipoString();
            }
            else
            {
                using (db.setComm(qry_getEventoNom))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        nom_evento_actual = Convert.ToString(db.getReader()["nombre"]);
                        ID_evento_actual = Convert.ToInt32(db.getReader()["ID"]);
                    }
                    SesionUsuario.setEvento(nom_evento_actual);
                    SesionUsuario.setIDEvento(ID_evento_actual);
                }
                changeEventTitle();
            }
            
            db.closeConn();

            // Remove elements if not administrator
            if (SesionUsuario.getUserTipo() == 2)
            {
                NuevoUsuario.Visibility = Visibility.Collapsed;
                EliminarUsuario.Visibility = Visibility.Collapsed;
            }
            nombreUsuario.Text = SesionUsuario.getUser();
                
        }
        public void changeEventTitle()
        {
            nombreEvento.Text = "";
            nombreEvento.Text = SesionUsuario.getUserTipoString() + " - Evento Actual: " +
                    SesionUsuario.getEvento() +
                    ". ID: " + SesionUsuario.getIDEvento();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void AnadirAuto_Click(object sender, RoutedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new AgregarAuto();
            GridMain.Children.Add(usc);
        }

        private void AnadirSistema_Click(object sender, RoutedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new AgregarSistema();
            GridMain.Children.Add(usc);
        }

        private void addEvento_Click(object sender, RoutedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new AgregarEvento();
            GridMain.Children.Add(usc);
        }

        private void AnadirFuncion_Click(object sender, RoutedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new NuevaFuncion();
            GridMain.Children.Add(usc);
        }

        private void AnadirUsuario_Click(object sender, RoutedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new NuevoUsuario();
            GridMain.Children.Add(usc);
        }

        private void EliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new EliminarUsuario();
            GridMain.Children.Add(usc);
        }

        private void CambiarEvento_Click(object sender, RoutedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new CambiarEvento();
            GridMain.Children.Add(usc);
        }

        private void InfoUsuario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UID: " + SesionUsuario.getUserID() +
                     ", UN: " + SesionUsuario.getUser() +
                     ", UT: " + SesionUsuario.getUserTipo() +
                     ", EV: " + SesionUsuario.getEvento() +
                     ", EVID: " + SesionUsuario.getIDEvento() 
            );
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    usc = new UserControl1_test();
                    GridMain.Children.Add(usc);
                    break;
                case "EditFunciones":
                    usc = new UserControl2_test();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }

    }
}
