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
    /// Lógica de interacción para EditFunciones.xaml
    /// </summary>
    public partial class EditFunciones : UserControl
    {
        DB db;

        int selectedFilter, firstFilter, secondFilter, thirdFilter;
        int systemID = -1, autoID = -1, funktionID;
        List<ComboBoxPairsFilter> cbp_filters;
        List<ComboBoxPairsBrowseAutos> cbp_browseAutos;
        List<CheckBoxPairsSistemas> cbp_browseSystems;
        public EditFunciones()
        {
            db = new DB();
            cbp_filters = new List<ComboBoxPairsFilter>();
            cbp_filters.Add(new ComboBoxPairsFilter(
                      "1",
                      "Autos"
                    ));
            cbp_filters.Add(new ComboBoxPairsFilter(
                     "2",
                     "Sistemas"
                   ));
            InitializeComponent();
            cb_searchFilter.DisplayMemberPath = "option";
            cb_searchFilter.SelectedValuePath = "value";

            cb_searchFilter.ItemsSource = cbp_filters;
        }
        private void cb_SF_DropDownClosed(object sender, EventArgs e)
        {
            // clear childs of stackpanel with systems
            if (cb_searchFilter.SelectedValue == null)
                return;
            selectedFilter = Convert.ToInt32(
                cb_searchFilter.SelectedValue.ToString());
            switch (selectedFilter)
            {
                // autos
                case 1:
                    // fill autos
                    FirstFilter.Items.Clear();
                    fillCars(1, 0);
                    break;
                // systems
                case 2:
                    FirstFilter.Items.Clear();
                    fillSystems(1, 0);
                    break;
            }
        }

        private void cb_FF_DropDownClosed(object sender, EventArgs e)
        {           
            if (FirstFilter.SelectedValue == null)
                return;
            //auto or system chosen
            firstFilter = Convert.ToInt32(
                FirstFilter.SelectedValue.ToString());

            //check what was selected 
            switch (selectedFilter)
            {
                // autos
                case 1:
                    // fill Systems
                    autoID = firstFilter;
                    SecondFilter.Items.Clear();
                    fillSystems(2, firstFilter);
                    break;
                // systems
                case 2:
                    systemID = firstFilter;
                    SecondFilter.Items.Clear();
                    fillCars(2, firstFilter);
                    break;
            }
        }

        private void cb_SecF_DropDownClosed(object sender, EventArgs e)
        {

            if (SecondFilter.SelectedValue == null)
                return;
            secondFilter = Convert.ToInt32(
                SecondFilter.SelectedValue.ToString());

            // select all functions that belong to the system,
            // and select edit campo funcion where funktion is selected
            // and auto is selected
            //auto or system chosen

            // systemid or autoid is stored, get the system or auto id
            // when the second item is chosen

        }

        public void fillCars(int option, int id)
        {
            cbp_browseAutos = new List<ComboBoxPairsBrowseAutos>();
            string qry_getAutos = "SELECT ID, modelo FROM autos";
            // query for second combobox
            string qry_getAutosWSystemID =
                "SELECT autos.ID AS ID, autos.modelo AS modelo ";
            qry_getAutosWSystemID += "FROM autos INNER JOIN ";
            qry_getAutosWSystemID +=
                "rel_autos_sist ON autos.ID = rel_autos_sist.autos_ID";
            qry_getAutosWSystemID += " WHERE sistema_ID = " + id;
         
            db.openConn();
            switch (option)
            {
                case 1:
                    using (db.setComm(qry_getAutos))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            cbp_browseAutos.Add(new ComboBoxPairsBrowseAutos(
                              Convert.ToString(db.getReader()["ID"]),
                              Convert.ToString(db.getReader()["modelo"])
                            ));
                        }
                    }                 
                    FirstFilter.DisplayMemberPath = "modelo";
                    FirstFilter.SelectedValuePath = "ID";
                    FirstFilter.ItemsSource = cbp_browseAutos;
                    break;
                    // got to be filled the second filter with autos
                case 2:
                    using (db.setComm(qry_getAutosWSystemID))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            cbp_browseAutos.Add(new ComboBoxPairsBrowseAutos(
                              Convert.ToString(db.getReader()["ID"]),
                              Convert.ToString(db.getReader()["modelo"])
                            ));
                        }
                    }

                    SecondFilter.DisplayMemberPath = "modelo";
                    SecondFilter.SelectedValuePath = "ID";
                    SecondFilter.ItemsSource = cbp_browseAutos;
                    break;
            }
            db.closeConn();
        }

        public void fillSystems(int option, int id)
        {
            cbp_browseSystems = new List<CheckBoxPairsSistemas>();
            string qry_getSistemas = "SELECT ID, nombre FROM sistema";
            // get systems of this car
            string qry_getSistemsWAutoID =
                "SELECT sistema.ID AS ID, sistema.nombre AS nombre ";
            qry_getSistemsWAutoID += "FROM sistema INNER JOIN ";
            qry_getSistemsWAutoID +=
                "rel_autos_sist ON sistema.ID = rel_autos_sist.sistema_ID ";
            qry_getSistemsWAutoID += "WHERE autos_ID = " + id;
            db.openConn();
            switch (option)
            {
                // autos
                case 1:
                    using (db.setComm(qry_getSistemas))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            cbp_browseSystems.Add(new CheckBoxPairsSistemas(
                              Convert.ToString(db.getReader()["ID"]),
                              Convert.ToString(db.getReader()["nombre"])
                            ));
                        }
                    }
                    FirstFilter.DisplayMemberPath = "nombre";
                    FirstFilter.SelectedValuePath = "ID";
                    FirstFilter.ItemsSource = cbp_browseSystems;
                    break;
                // systems
                case 2:
                    using (db.setComm(qry_getSistemsWAutoID))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            cbp_browseSystems.Add(new CheckBoxPairsSistemas(
                              Convert.ToString(db.getReader()["ID"]),
                              Convert.ToString(db.getReader()["modelo"])
                            ));
                        }
                    }
                    SecondFilter.DisplayMemberPath = "modelo";
                    SecondFilter.SelectedValuePath = "ID";
                    SecondFilter.ItemsSource = cbp_browseAutos;
                    break;
            }
            db.closeConn();
        }

    }

    public class ComboBoxPairsFilter
    {
        public string value { get; set; }
        public string option { get; set; }

        public ComboBoxPairsFilter(string value, string option)
        {
            this.value = value;
            this.option = option;
        }
    }
}
