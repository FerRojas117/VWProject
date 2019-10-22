using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    /// Lógica de interacción para AgregarSistema.xaml
    /// </summary>
    public partial class AgregarSistema : UserControl
    {
        DB db;
        List<ComboBoxPairsBrowseAutos> cbp;
        List<ComboBoxPairsBrowseAutos> cbp_Autos;
        List<CheckBoxPairsSistemas> cbp_browseSistema;
        int affectedRows = 0;
        string qry_getNoSistemas = "SELECT COUNT(*) AS numAutos FROM autos WHERE isActive = 1";

        public AgregarSistema()
        {
            cbp = new List<ComboBoxPairsBrowseAutos>();
            InitializeComponent();
            // put elements not visible till we click on the buttons
            noAutos.Visibility = Visibility.Collapsed;
            // controles de añadir Sistema 
            labelSistema.Visibility = Visibility.Collapsed;
            nombreSistema.Visibility = Visibility.Collapsed;
            crearSistema .Visibility = Visibility.Collapsed;
            // controles de Buscar Sistema
            buscarSistema.Visibility = Visibility.Collapsed;
            relacionarSistema.Visibility = Visibility.Collapsed;
            db = new DB();
            fillAutos();
        }


        private void addSistema_Click(object sender, RoutedEventArgs e)
        {
            buscarSistema.Visibility = Visibility.Collapsed;
            relacionarSistema.Visibility = Visibility.Collapsed;

            labelSistema.Visibility = Visibility.Visible;
            nombreSistema.Visibility = Visibility.Visible;
            crearSistema.Visibility = Visibility.Visible;
            fillStackWithAllAutos();

        }

        private void searchSistema_Click(object sender, RoutedEventArgs e)
        {
            labelSistema.Visibility = Visibility.Collapsed;
            nombreSistema.Visibility = Visibility.Collapsed;
            crearSistema.Visibility = Visibility.Collapsed;

            buscarSistema.Visibility = Visibility.Visible;
            relacionarSistema.Visibility = Visibility.Visible;

            fillSistema();
        }

        public void fillSistema()
        {
            cbp_browseSistema = new List<CheckBoxPairsSistemas>();
            string qry_getEventos = "SELECT ID, nombre FROM sistema  WHERE isActive = 1";
            db.openConn();
            using (db.setComm(qry_getEventos))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    cbp_browseSistema.Add(new CheckBoxPairsSistemas(
                      Convert.ToString(db.getReader()["ID"]),
                      Convert.ToString(db.getReader()["nombre"])
                    ));
                }
            }
            db.closeConn();
            buscarSistema.DisplayMemberPath = "nombre";
            buscarSistema.SelectedValuePath = "ID";
            buscarSistema.ItemsSource = cbp_browseSistema;


            // clear childs of stackpanel with systems
            stackAutos.Children.Clear();

        }

        public void fillAutos()
        {
            db.openConn();
            using (db.setComm(qry_getNoSistemas))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    affectedRows = Convert.ToInt32(db.getReader()["numAutos"]);
                }
            }
            if (affectedRows == 0)
            {
                db.sendMBandCloseConn("No hay Autos registrados. Por el momento");
                noAutos.Visibility = Visibility.Visible;
            }
            // llenar el stack con los autos encontrados
            else
            {
                string qry_getSistemas = "SELECT ID, modelo FROM autos  WHERE isActive = 1";
                using (db.setComm(qry_getSistemas))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        cbp.Add(new ComboBoxPairsBrowseAutos(
                            Convert.ToString(db.getReader()["ID"]),
                            Convert.ToString(db.getReader()["modelo"])
                        ));
                    }
                }
                fillStackWithAllAutos();
                db.closeConn();
            }
        }

        // append all available systems, put in another function
        public void fillStackWithAllAutos()
        {
            stackAutos.Children.Clear();
            foreach (ComboBoxPairsBrowseAutos cpe in cbp)
            {
                CheckBox cb = new CheckBox();
                cb.Name = "_" + cpe.ID;
                cb.Content = cpe.modelo;
                stackAutos.Children.Add(cb);
            }
        }

        private void buscarSistema_DropDownClosed(object sender, EventArgs e)
        {
            // clear childs of stackpanel with systems
            if (buscarSistema.SelectedValue == null)
                return;

            stackAutos.Children.Clear();
            cbp_Autos = new List<ComboBoxPairsBrowseAutos>();
            string ID_selectedSystem = buscarSistema.SelectedValue.ToString();
            db.openConn();
            
            
            string qry_getSistemas = "SELECT * FROM autos WHERE ID NOT IN(";
            qry_getSistemas += "SELECT autos_ID FROM rel_autos_sist ";
            qry_getSistemas += "WHERE sistema_ID = " + ID_selectedSystem + " ) AND isActive = 1";

            using (db.setComm(qry_getSistemas))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    cbp_Autos.Add(new ComboBoxPairsBrowseAutos(
                        Convert.ToString(db.getReader()["ID"]),
                        Convert.ToString(db.getReader()["modelo"])
                    ));
                }
            }
            // we fill stackpanel with systems that are not related with this car
            fillAutosExcept();
            db.closeConn();
        }

        public void fillAutosExcept()
        {
            foreach (ComboBoxPairsBrowseAutos cpe in cbp_Autos)
            {
                CheckBox cb = new CheckBox();
                cb.Name = "_" + cpe.ID;
                cb.Content = cpe.modelo;
                stackAutos.Children.Add(cb);
            }
        }

        private void crearSistema_Click(object sender, RoutedEventArgs e)
        {
            string nombreSist = nombreSistema.Text;
            MessageBox.Show(nombreSist);
            string qry_insAuto = "INSERT INTO sistema (nombre, isActive) VALUES ('" +
                nombreSist + "', 1)";
            db.openConn();
            // if car was not inserted or if there is no systems registered, then
            if (affectedRows == 0 || getNumSelectedCB() == 0)
            {
                MessageBoxResult result = MessageBox.Show(
                   "Estás seguro que quieres crear este sistema sin relacion con algún auto?",
                   "Crear sistema",
                   MessageBoxButton.YesNo
                );
                switch (result)
                {
                    case MessageBoxResult.Yes:

                        using (db.setComm(qry_insAuto))
                        {
                            affectedRows = db.getComm().ExecuteNonQuery();
                        }
                        if (affectedRows == 0)
                        {
                            db.sendMBandCloseConn("No se pudo crear Sistema. Inténtalo de nuevo");
                            return;
                        }
                        db.sendMBandCloseConn("Sistema agregado exitosamente a la base de datos." +
                            " No se relacionó con ningún auto.");
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Sistema no creado.", "Crear Sistema");
                        db.closeConn();
                        break;
                }
            }
            else
            {
                using (db.setComm(qry_insAuto))
                {
                    affectedRows = db.getComm().ExecuteNonQuery();
                }
                if (affectedRows == 0)
                {
                    db.sendMBandCloseConn("No se pudo crear Sistema. Inténtalo de nuevo");
                    return;
                }
                // We get the ID of the last inserted car successfully,
                // we then have to relate with  a system
                string sql = "SELECT last_insert_rowid()";
                SQLiteCommand cmd = new SQLiteCommand(sql, db.getConn());
                int lastInsertSystemID = Convert.ToInt32(cmd.ExecuteScalar());

                sistemsRelations(lastInsertSystemID);


                db.sendMBandCloseConn("Sistema agregado exitosamente a la base de datos." +
                           " Se relacionó con varios autos.");
            }
        }

        public void sistemsRelations(int systemID)
        {
            // Insert relation of auto and system
            string qry_insAutoSystem = "INSERT INTO rel_autos_sist (autos_ID, sistema_ID) VALUES ";
            qry_insAutoSystem += "(";
            string qry_insAutoSystemMod;
            string ID_currentCB;

            // insert code for relation of system
            foreach (CheckBox cb in stackAutos.Children)
            {
                // we get the id of the name of each cb, combobox, 
                // of course we have to know if cb was checked
                if (cb.IsChecked.HasValue && cb.IsChecked.Value == true)
                {
                    ID_currentCB = cb.Name.ToString();
                    ID_currentCB = ID_currentCB.Trim(new char[] { '_' });
                    // we append to the list of IDS of checked comboboxes
                    // could not append several insert values so we do one insert at a time
                    qry_insAutoSystemMod = qry_insAutoSystem + ID_currentCB + ", " + systemID + ")";
                    MessageBox.Show("QRY: " + qry_insAutoSystemMod);
                    using (db.setComm(qry_insAutoSystemMod))
                    {
                        affectedRows = db.getComm().ExecuteNonQuery();
                    }
                    if (affectedRows == 0)
                    {
                        db.sendMBandCloseConn("No se pudo crear las relaciones pertinentes de sistemas y autos.");
                        return;
                    }
                }
            }
        }

        private void relacionarSistema_Click(object sender, RoutedEventArgs e)
        {
            if (buscarSistema.SelectedValue == null)
                return;
            string ID_selectedSystem = buscarSistema.SelectedValue.ToString();
            db.openConn();
            sistemsRelations(Convert.ToInt32(ID_selectedSystem));
            db.sendMBandCloseConn("Sistema relacionado exitosamente con varios autos.");
        }

        public int getNumSelectedCB()
        {
            int numSistemas = 0;
            IEnumerable<CheckBox> selectedBoxes =
            stackAutos.Children.OfType<CheckBox>()
            .Where(cb => cb.IsChecked.Value);

            foreach (CheckBox box in selectedBoxes)
            {
                numSistemas += 1;
            }
            return numSistemas;
        }

    }
}
