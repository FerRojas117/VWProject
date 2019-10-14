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
        List<Item2Edit> items;
        List<CheckBoxPairsSistemas> cbpairs;
        List<Item2EditAuto> restore;

        int whichItem = 0;
        public Administracion()
        {
            InitializeComponent();
            db = new DB();
            items = new List<Item2Edit>();
            restore = new List<Item2EditAuto>();

            cbpairs = new List<CheckBoxPairsSistemas>();
            cbpairs.Add(new CheckBoxPairsSistemas("Autos", "Autos" ));
            cbpairs.Add(new CheckBoxPairsSistemas("Sistemas", "Sistemas"));
            cbpairs.Add(new CheckBoxPairsSistemas("Funciones", "Funciones"));
            edit.DisplayMemberPath = "nombre";
            edit.SelectedValuePath = "ID";
            edit.ItemsSource = cbpairs;
        }

        private void buscar_DropDownClosed(object sender, EventArgs e)
        {
            // clear childs of stackpanel with systems
            if (edit.SelectedValue == null)
                return;

            items.Clear();

            string selectedEdition = edit.SelectedValue.ToString();
            MessageBox.Show(selectedEdition);
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
                    string qry_getFunciones = "SELECT ID, nombre FROM funktion";
                    fillItems2Edit(qry_getFunciones, whichItem);
                    break;
            }
        }

        public void fillItems2Edit(string getItems, int autoOrSystem)
        {
            string qry_RestoreItems = "SELECT ";
            int numRegisters = 0;
            db.openConn();
            if(autoOrSystem == 1)
            {
                using (db.setComm(getItems))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        items.Add(
                            new Item2Edit
                            {
                                ID = Convert.ToString(db.getReader()["ID"]),
                                nombre = Convert.ToString(db.getReader()["modelo"])
                            }
                        );
                    }
                }
                //  if car was selected, get elements that are notactive
                qry_RestoreItems += "ID, modelo FROM autos WHERE isActive = 0";
                using (db.setComm(qry_RestoreItems))
                {
                    numRegisters = db.getComm().ExecuteNonQuery();
                }
                if (numRegisters == 0)
                {
                    noItems.Visibility = Visibility.Visible;
                }
                else
                {
                    using (db.setComm(qry_RestoreItems))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            restore.Add(
                                new Item2EditAuto
                                {
                                    ID = Convert.ToString(db.getReader()["ID"]),
                                    modelo = Convert.ToString(db.getReader()["modelo"])
                                }
                            );
                        }

                    }
                    // fill stackpanel with elements that can be restored
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
                            new Item2Edit
                            {
                                ID = Convert.ToString(db.getReader()["ID"]),
                                nombre = Convert.ToString(db.getReader()["nombre"])
                            }
                        );
                    }
                }

                // check what was selected to bring to stackpanel the items      

            }            
            db.closeConn();

            ItemsCB.DisplayMemberPath = "nombre";
            ItemsCB.SelectedValuePath = "ID";
            ItemsCB.ItemsSource = items;

          
        }

    }

    public class Item2Edit
    {
        public string ID { get; set; }
        public string nombre { get; set; }
    }

    public class Item2EditAuto
    {
        public string ID { get; set; }
        public string modelo { get; set; }
    }
}
