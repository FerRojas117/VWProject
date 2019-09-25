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
        DB tDB;
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

            Bemerkung bmrkng;

            foreach (Funktion f in funktions)
            {
                string qry_bmrFunktion = "SELECT * FROM bemerkung WHERE funktion_ID = ";
                qry_bmrFunktion += f.ID;
                qry_bmrFunktion += " AND evento_ID = ";
                qry_bmrFunktion += SesionUsuario.getIDEvento();


                using (db.setComm(qry_bmrFunktion))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        bmrkng = new Bemerkung()
                        {
                            ID = Convert.ToString(db.getReader()["ID"]),
                            bemerkung = Convert.ToString(db.getReader()["bemerkung"]),
                            funktion_ID = Convert.ToString(db.getReader()["funktion_ID"]),
                            editado_por = Convert.ToString(db.getReader()["editado_por"]),
                            evento_ID = Convert.ToString(db.getReader()["evento_ID"])
                        };
                        // store bemerkungen in list
                        f.addBemerkungFuncion(bmrkng);
                    }
                }
                f.dgBemerkungen.ItemsSource = f.bemFuncion;
               
            }
            
            db.closeConn();

        }
        public displayFunciones()
        {
            db = new DB();
            InitializeComponent();
            LoadCollectionData();
            createDataGrid();
            //DataContext = person;
            
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void CheckAtrrClass(object sender, RoutedEventArgs e)
        {

            //dg.Columns[0].Visibility = Visibility.Hidden;
            /*
            string nombresAtributos = "";
            foreach(Funktion f in funktions)
            {
                nombresAtributos += f.nombre + ", ";
            }
            MessageBox.Show(nombresAtributos);
            */
        }

        public void createDataGrid()
        {
            Style st = FindResource("styleA") as Style;
            StackPanel sp_btnBemerkungen = new StackPanel();
            sp_btnBemerkungen.Margin = new Thickness(5, 45, 0, 0);
            // check how to format buttons 
            //sp_btnBemerkungen.Resources["Style"]
            StackPanel sp_bemerkungen = new StackPanel();
           
            foreach (Funktion f in funktions)
            {
                Button btn = new Button();
                btn.Content = string.Format("Bemerkung: {0}", f.nombre);
                btn.Name = "_" + Convert.ToString(f.ID);
                btn.Style = st;
                btn.Click += new RoutedEventHandler(btn_Click);
                sp_btnBemerkungen.Children.Add(btn);
                sp_bemerkungen.Children.Add(f.dgBemerkungen);
                f.dgBemerkungen.Visibility = Visibility.Collapsed;
            }
            
            dg.ItemsSource = funktions;
            
            panelPrueba.Children.Add(dg);
            panelPrueba.Children.Add(sp_btnBemerkungen);
            panelPrueba.Children.Add(sp_bemerkungen);
        }


        void btn_Click(object sender, RoutedEventArgs e)
        {

            Button buttonThatWasClicked = (Button)sender;
            int buttonIDSelected = 
                Convert.ToInt32(buttonThatWasClicked.Name.Trim(new char[] { '_'}));
            foreach(Funktion f in funktions)
            {
                if (f.ID == buttonIDSelected)
                    f.dgBemerkungen.Visibility = Visibility.Visible;
                else f.dgBemerkungen.Visibility = Visibility.Collapsed;
            }
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
        public List<Bemerkung> bemFuncion;
        public DataGrid dgBemerkungen;

        public Funktion()
        {
            bemFuncion = new List<Bemerkung>();
            dgBemerkungen = new DataGrid();
        }

        public void addBemerkungFuncion(Bemerkung b)
        {
            bemFuncion.Add(b);
        }

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
