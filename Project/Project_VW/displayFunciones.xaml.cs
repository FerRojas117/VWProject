using System;
using System.ComponentModel;
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
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;

namespace Project_VW
{
    /// <summary>
    /// Lógica de interacción para displayFunciones.xaml
    /// </summary>
    public partial class displayFunciones : UserControl
    {
        DB db;
        private string valorTB;
        Funcion f;
        string qry_getF = "SELECT * FROM funktion";
        SQLiteDataAdapter mAdapter;
        DataTable mTable;
        List<Funktion> funktions = new List<Funktion>();
        DataGrid dg = new DataGrid();
        //Person person = new Person { Name = "Salman", Age = 26 };
        private void LoadCollectionData()
        {
           
            db.openConn();
            using (db.setComm(qry_getF))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    funktions.Add(new Funktion()
                    {
                        ID = Convert.ToInt32(db.getReader()["ID"]),
                        nombre = Convert.ToString(db.getReader()["nombre"]),
                        NAR = Convert.ToString(db.getReader()["NAR"]),
                        RDW = Convert.ToString(db.getReader()["RDW"]),
                        Gesetz= Convert.ToString(db.getReader()["Gesetz"]),
                        KW = Convert.ToString(db.getReader()["KW"]),
                        Jahr = Convert.ToString(db.getReader()["Jahr"]),
                        descripcion = Convert.ToString(db.getReader()["descripcion"]),
                        sistema_ID = Convert.ToString(db.getReader()["sistema_ID"]),
                        editado_por = Convert.ToString(db.getReader()["editado_por"])
                    }); 
                }
            }

        }
        public displayFunciones()
        {
            db = new DB();
            InitializeComponent();
            LoadCollectionData();
            createDataGrid();
            //DataContext = person;
        }

        private void CheckAtrrClass(object sender, RoutedEventArgs e)
        {
            string nombresAtributos = "";
            foreach(Funktion f in funktions)
            {
                nombresAtributos += f.nombre + ", ";
            }
            MessageBox.Show(nombresAtributos);
        }

        public void createDataGrid()
        {

            /*
            // Test this with a class and update to se if it works 
            //  with enumerable object
            DataGrid dg = new DataGrid();
            
            //string qry_getF = "SELECT * FROM funktion";
            db.openConn();
            mAdapter = new SQLiteDataAdapter(db.setComm(qry_getF));
            mTable = new DataTable("funktion");
            mAdapter.Fill(mTable);
            db.closeConn();*/
            
            dg.ItemsSource = funktions;
            //dg.AutoGenerateColumns = false;
            
            //mAdapter.Update(mTable);
            
            
            panelPrueba.Children.Add(dg);
        }

        /*
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string message = person.Name + " is " + person.Age;
            MessageBox.Show(message);
        }
        */

    }


    public class Funktion
    {
        public int ID { get; set; }
        public string nombre { get; set; }
        public string NAR { get; set; }
        public string RDW { get; set; }
        public string Gesetz { get; set; }
        public string KW { get; set; }
        public string Jahr { get; set; }
        public string descripcion { get; set; }
        public string sistema_ID { get; set; }
        public string editado_por { get; set; }


    }
    public class Person
    {

        private string nameValue;

        public string Name
        {
            get { return nameValue; }
            set { nameValue = value; }
        }

        private double ageValue;

        public double Age
        {
            get { return ageValue; }

            set
            {
                if (value != ageValue)
                {
                    ageValue = value;
                }
            }
        }

    }
}
