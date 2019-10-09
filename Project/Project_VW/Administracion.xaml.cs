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
    /// Lógica de interacción para Administracion.xaml
    /// </summary>
    public partial class Administracion : UserControl
    {
        DB db;
        Dictionary<string, string> items;
        int whichItem = 0;
        public Administracion()
        {
            InitializeComponent();
            db = new DB();
            items = new Dictionary<string, string>();
        }

        private void buscar_DropDownClosed(object sender, EventArgs e)
        {
            // clear childs of stackpanel with systems
            if (edit.SelectedValue == null)
                return;

            Content.Children.Clear();

            string selectedEdition = edit.SelectedValue.ToString();

            switch(selectedEdition)
            {
                case "Autos":
                    string qry_getAutos = "SELECT ID, modelo FROM autos";
                    whichItem = 1;
                    fillItems2Edit(qry_getAutos, whichItem);
                    break;
                case "Funciones":
                    whichItem = 2;
                    string qry_getSistemas = "SELECT ID, nombre FROM sistema";
                    fillItems2Edit(qry_getSistemas, whichItem);
                    break;
                case "Sistemas":
                    whichItem = 3;
                    string qry_getFunciones = "SELECT ID, nombre FROM fukntion";
                    fillItems2Edit(qry_getFunciones, whichItem);
                    break;
            }
        }

        public void fillItems2Edit(string getItems, int autoOrSystem)
        {

            db.openConn();
            if(autoOrSystem == 1)
            {
                using (db.setComm(getItems))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {

                        items.Add(
                            Convert.ToString(db.getReader()["ID"]),
                            Convert.ToString(db.getReader()["modelo"])
                        );
                    }
                }
            }
            else
            {
                using (db.setComm(getItems))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {

                        items.Add(
                            Convert.ToString(db.getReader()["ID"]),
                            Convert.ToString(db.getReader()["nombre"])
                        );
                    }
                }
            }            
            db.closeConn();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

    }

    public class Item2Edit
    {
        public string ID { get; set; }
        public string nombre { get; set; }
    }
}
