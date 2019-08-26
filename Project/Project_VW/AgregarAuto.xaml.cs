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



/// <summary>
///  still missing :
///  relation of new inserted car 
///  show existing cars to relate to a system
///  show systems that are NOT related to a specific car
///  when looking for a car
/// </summary>



namespace Project_VW
{
    /// <summary>
    /// Lógica de interacción para AgregarAuto.xaml
    /// </summary>
    public partial class AgregarAuto : UserControl
    {
        DB db;
        List<CheckBoxPairsSistemas> cbp;
        int affectedRows = 0;

        public AgregarAuto()
        {

            cbp = new List<CheckBoxPairsSistemas>();
            string qry_getNoSistemas = "SELECT COUNT(*) AS numSistemas FROM sistema";
            InitializeComponent();
            // put elements not visible till we click on the buttons
            noSystems.Visibility = Visibility.Collapsed;
            // controles de añadir auto 
            labelAuto.Visibility = Visibility.Collapsed;
            nombreAuto.Visibility = Visibility.Collapsed;
            crearAuto.Visibility = Visibility.Collapsed;
            // controles de Buscar Auto
            buscarAuto.Visibility = Visibility.Collapsed;
            relacionarAuto.Visibility = Visibility.Collapsed;

            db = new DB();
            db.openConn();
            using (db.setComm(qry_getNoSistemas))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    affectedRows = Convert.ToInt32(db.getReader()["numSistemas"]);
                }
            }
            if (affectedRows == 0)
            {
                db.sendMBandCloseConn("No hay Sistemas registrados. Por el momento");
                noSystems.Visibility = Visibility.Visible;
            }
            // llenar el stack con los sistemas encontrados
            else
            {
                string qry_getSistemas = "SELECT ID, nombre FROM sistema";
                using (db.setComm(qry_getSistemas))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        cbp.Add(new CheckBoxPairsSistemas(
                            Convert.ToString(db.getReader()["ID"]),
                            Convert.ToString(db.getReader()["nombre"])
                        ));
                    }
                }
                foreach(CheckBoxPairsSistemas cpe in cbp)
                {
                    CheckBox cb = new CheckBox();
                    cb.Name = "_" + cpe.ID;
                    cb.Content = cpe.nombre;
                    stackSystems.Children.Add(cb);
                }
                db.closeConn();
            }
        }

        private void addAuto_Click(object sender, RoutedEventArgs e)
        {
            buscarAuto.Visibility = Visibility.Collapsed;
            relacionarAuto.Visibility = Visibility.Collapsed;

            labelAuto.Visibility = Visibility.Visible;
            nombreAuto.Visibility = Visibility.Visible;
            crearAuto.Visibility = Visibility.Visible;
        }

        private void searchAuto_Click(object sender, RoutedEventArgs e)
        {
            labelAuto.Visibility = Visibility.Collapsed;
            nombreAuto.Visibility = Visibility.Collapsed;
            crearAuto.Visibility = Visibility.Collapsed;

            buscarAuto.Visibility = Visibility.Visible;
            relacionarAuto.Visibility = Visibility.Visible;
        }

        private void crearAuto_Click(object sender, RoutedEventArgs e)
        {
            string nombreModeloAuto = nombreAuto.Text;
            MessageBox.Show(nombreModeloAuto);
            string qry_insAuto = "INSERT INTO autos (modelo) VALUES ('" +
                nombreModeloAuto + "')";
            db.openConn();
            // if car was not inserted or if there is no systems registered, then
            if ( affectedRows == 0 ||  getNumSelectedCB() == 0)
            {
                MessageBoxResult result = MessageBox.Show(
                   "Estás seguro que quieres crear este auto sin relacion con algún sistema?",
                   "Crear auto",
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
                            db.sendMBandCloseConn("No se pudo crear Auto. Inténtalo de nuevo");
                            return;
                        }
                        db.sendMBandCloseConn("Auto agregado exitosamente a la base de datos."+
                            " No se relacionó con ningún evento.");
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Auto no creado.", "Crear Auto");
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
                    db.sendMBandCloseConn("No se pudo crear AUTO. Inténtalo de nuevo");
                    return;
                }
                // We get the ID of the last inserted car successfully,
                // we then have to relate with  a system
                string sql = "SELECT last_insert_rowid()";
                SQLiteCommand cmd = new SQLiteCommand(sql, db.getConn());
                int lastID = Convert.ToInt32(cmd.ExecuteScalar());

                // insert code for relation of system

                db.sendMBandCloseConn("Auto Agregado exitosamente. " + "ID: " + lastID);
                
            }
        }

        private void relacionarAuto_Click(object sender, RoutedEventArgs e)
        {
            // we have to check if the relation extists 
            // with index of car and systems, and discriminate info

            buscarAuto.Visibility = Visibility.Visible;
            relacionarAuto.Visibility = Visibility.Visible;
        }

        public int getNumSelectedCB()
        {
            int numSistemas = 0;
            IEnumerable<CheckBox> selectedBoxes =
            stackSystems.Children.OfType<CheckBox>()
            .Where(cb => cb.IsChecked.Value);

            foreach (CheckBox box in selectedBoxes)
            {
                numSistemas += 1;
            }
            return numSistemas;
        }
    }

    public class CheckBoxPairsSistemas
    {
        public string ID { get; set; }
        public string nombre { get; set; }


        public CheckBoxPairsSistemas(string ID_P, string user_p)
        {
            ID = ID_P;
            nombre = user_p;
        }
    }
}
