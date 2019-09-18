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
    /// Lógica de interacción para Index.xaml
    /// </summary>
    public partial class Index : UserControl
    {
        DB db;
        DB getFunc;
        List<ComboBoxPairsBrowseAutos> cbp_browseAutos;
        List<Sistema> sistemasDelAuto;
        List<Expander> expList;
        //List<DataGrid> gridOfEachSystem;
        public Index()
        {
            InitializeComponent();
            db = new DB();
            fillCars();
        }

        public void fillCars()
        {
            cbp_browseAutos = new List<ComboBoxPairsBrowseAutos>();
            string qry_getEventos = "SELECT ID, modelo FROM autos";
            db.openConn();
            using (db.setComm(qry_getEventos))
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
            db.closeConn();
            filtroAutos.DisplayMemberPath = "modelo";
            filtroAutos.SelectedValuePath = "ID";
            filtroAutos.ItemsSource = cbp_browseAutos;
        }

        
       
        public void fAuto_dropdownClosed(object sender, EventArgs e)
        {
            // if nothing was chosen then return
            if (filtroAutos.SelectedValue == null)
                return;

            // funktion_ID = 1 AND evento_ID = 6 AND auto_ID = 18; "

            string ID, nombre, NAR, RDW, Gesetz, KW, Jahr, descripcion;
            string einsatz, abgesichert;

            // objeto de sistema de la base de datos

            string ID_selectedCar = filtroAutos.SelectedValue.ToString();

            SistemasAutos.Children.Clear();
            int registerCounter = 0;
            sistemasDelAuto = new List<Sistema>();
            List<Bemerkung> bemerkungsFunktion;
            List<Funcion> funktionSistemas;
            Sistema st;
            Funcion ft;
            Bemerkung bmrkng;
            Edit_Campos_Funcion ecf = null;
            

            int ID_func;

            // get systems that relate to a car
            // We will filter bemerkungen and edit campos funktion later


            // COUNT IF THERE IS SYSTEMS
            string qry_sistDeAutoCount = "SELECT COUNT(sistema.ID) AS numDeSist ";
            qry_sistDeAutoCount += "FROM sistema ";
            qry_sistDeAutoCount += "INNER JOIN rel_autos_sist ";
            qry_sistDeAutoCount += "ON sistema.ID = rel_autos_sist.sistema_ID ";
            qry_sistDeAutoCount += "WHERE autos_ID = ";
            qry_sistDeAutoCount += ID_selectedCar;


            string qry_sistDeAuto = "SELECT sistema.ID, sistema.nombre FROM sistema ";
            qry_sistDeAuto += "INNER JOIN rel_autos_sist ";
            qry_sistDeAuto += "ON sistema.ID = rel_autos_sist.sistema_ID ";
            qry_sistDeAuto += "WHERE autos_ID = ";
            qry_sistDeAuto += ID_selectedCar;


            // get infromation of systems
            db.openConn();
            // count systems that relate to a car 
            using (db.setComm(qry_sistDeAutoCount))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    registerCounter = Convert.ToInt32(db.getReader()["numDeSist"]);
                }
            }
            // there is no systems related to a car 
            if(registerCounter <= 0)
            {
                db.sendMBandCloseConn("No hay Sistemas relacionados a este auto.");
                return;
            }

            sistemasDelAuto = new List<Sistema>();
            // read systems that relate to the chosen car

            using (db.setComm(qry_sistDeAuto))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    st = new Sistema(
                        Convert.ToInt32(db.getReader()["ID"]),
                        Convert.ToString(db.getReader()["nombre"])
                    );
                    sistemasDelAuto.Add(st);
                }
            }

        


            #region Get funktions of each system
            // GET Funktions from each System
            foreach (Sistema ptrSistema in sistemasDelAuto)
            {
                string qry_getFunctionsCount = "SELECT COUNT(*) AS numFuncSistema ";
                qry_getFunctionsCount += "FROM funktion WHERE sistema_ID = ";
                string qry_getFunctions = "SELECT * FROM funktion WHERE sistema_ID = ";
                qry_getFunctions += ptrSistema.ID;
                qry_getFunctionsCount += ptrSistema.ID;

                // check if system has funktions
                using (db.setComm(qry_getFunctionsCount))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        registerCounter = Convert.ToInt32(db.getReader()["numFuncSistema"]);
                    }
                }
                if(registerCounter <= 0)
                {
                    continue;
                }
                funktionSistemas = new List<Funcion>();
                // let's look for its information, its bemerkungen and its
                // einsatz and abgesichert fields
                getFunc = new DB();
                getFunc.openConn();

                #region Get Functions of each system
                using (getFunc.setComm(qry_getFunctions))
                {
                    getFunc.setReader();

                    while (getFunc.getReader().Read())
                    {
                        bemerkungsFunktion = new List<Bemerkung>();

                        ID = Convert.ToString(getFunc.getReader()["ID"]);
                        nombre = Convert.ToString(getFunc.getReader()["nombre"]);
                        NAR = Convert.ToString(getFunc.getReader()["NAR"]);
                        RDW = Convert.ToString(getFunc.getReader()["RDW"]);
                        Gesetz = Convert.ToString(getFunc.getReader()["Gesetz"]);
                        KW = Convert.ToString(getFunc.getReader()["KW"]);
                        Jahr = Convert.ToString(getFunc.getReader()["Jahr"]);
                        descripcion = Convert.ToString(getFunc.getReader()["descripcion"]);

                        Console.WriteLine(ID);
                        ft = new Funcion()
                        {
                            ID =  ID,
                            nombre = nombre,
                            NAR = NAR,
                            RDW = RDW,
                            Gesetz = Gesetz,
                            KW = KW,
                            Jahr = Jahr,
                            descripcion = descripcion
                        };

                        // we get infromation from funktion 
                        // we have to get information from bemerkungen of
                        // this funktion and its specific information
                        // about edit_campos_funcion


                        // count first if there is bemerkungen
                        string qry_bmrFunktionCount = "SELECT COUNT (*) as bmrkCount FROM bemerkung WHERE funktion_ID = ";
                        qry_bmrFunktionCount += ID;
                        qry_bmrFunktionCount += " AND evento_ID = ";
                        qry_bmrFunktionCount += SesionUsuario.getIDEvento();

                        using (db.setComm(qry_bmrFunktionCount))
                        {
                            db.setReader();
                            while (db.getReader().Read())
                            {
                                registerCounter = Convert.ToInt32(db.getReader()["bmrkCount"]);
                            }
                        }

                        // if there is no bemerkungen in this function, we wont 
                        // add a thing in the lsit from bemerkungen
                        if (registerCounter > 0) 
                        {
                            // filter bemerkungen by funktionID and eventID 

                            string qry_bmrFunktion = "SELECT * FROM bemerkung WHERE funktion_ID = ";
                            qry_bmrFunktion += ID;
                            qry_bmrFunktion += " AND evento_ID = ";
                            qry_bmrFunktion += SesionUsuario.getIDEvento();

                            using (db.setComm(qry_bmrFunktion))
                            {
                                db.setReader();
                                while (db.getReader().Read())
                                {
                                    bmrkng = new Bemerkung(
                                        Convert.ToString(db.getReader()["ID"]),
                                        Convert.ToString(db.getReader()["bemerkung"]),
                                        Convert.ToString(db.getReader()["funktion_ID"]),
                                        Convert.ToString(db.getReader()["editado_por"]),
                                        Convert.ToString(db.getReader()["evento_ID"])
                                    );
                                    // store bemerkungen in list
                                    ft.addBemerkungFuncion(bmrkng);
                                }
                            }                           
                        }

                        string qry_ECFCount = "SELECT COUNT(*) as existsECF ";
                        qry_ECFCount += "FROM edit_campos_funktion ";
                        qry_ECFCount += "WHERE funktion_ID = " + ID;
                        qry_ECFCount += " AND evento_ID = " + SesionUsuario.getIDEvento();
                        qry_ECFCount += " AND auto_ID = " + ID_selectedCar;

                        string qry_ECF = "SELECT * ";
                        qry_ECF += "FROM edit_campos_funktion ";
                        qry_ECF += "WHERE funktion_ID = " + ID;
                        qry_ECF += " AND evento_ID = " + SesionUsuario.getIDEvento();
                        qry_ECF += " AND auto_ID = " + ID_selectedCar;


                        // filter edit campos funcion 
                        // by funktion id, evento,ID and auto_ID

                        // FIRST COUNT IF THE TABLE EXISTS
                        using (db.setComm(qry_ECFCount))
                        {
                            db.setReader();
                            while (db.getReader().Read())
                            {
                                registerCounter = Convert.ToInt32(db.getReader()["existsECF"]);
                            }
                        }

                        // IF table does not exists, then insert table
                        // with values:
                        // in einsatz and abgesichert and edited_by ALL empty
                        // funktion_ID, the funktion that we are currently itearating
                        // evento_ID, the event that we are on
                        // and car_ID, the car id that the user has chosen.



                        if (registerCounter > 0)
                        {
                            using (db.setComm(qry_ECF))
                            {
                                db.setReader();
                                while (db.getReader().Read())
                                {
                                    ecf = new Edit_Campos_Funcion(
                                        Convert.ToString(db.getReader()["ID"]),
                                        Convert.ToString(db.getReader()["einsatz"]),
                                        Convert.ToString(db.getReader()["abgesichert"])
                                    );
                                    ft.addECFFuncion(ecf);
                                }
                            }
                        }
                        else ft.addECFFuncion(ecf);



                        // END information retrieval
                        ptrSistema.addFuncionSistema(ft);
                    }                
                }
               
                // add ItemsSource to GRID of funktions of each system
                ptrSistema.gvSystem.ItemsSource = ptrSistema.funkDeSistema;


                // add ItemsSource to GRID of EditCamposFunkt of each system
                

                #endregion
                getFunc.closeConn();
                foreach (Funcion f in ptrSistema.funkDeSistema)
                {
                    MessageBox.Show(f.ID);
                }
            }
            db.closeConn();

            #endregion

            // end of systems of car retrieval

            // put information in frontend
            showInformationOfCar();
        }

        public void showInformationOfCar()
        {
            // got to show everything
            expList = new List<Expander>();
            foreach (Sistema s in sistemasDelAuto)
            {
                Expander xpanderS = new Expander();
                xpanderS.Background = Brushes.Tan;
                xpanderS.Header = s.nombre;

                StackPanel spf = new StackPanel();
                //StackPanel spf = new StackPanel();
                spf.Orientation = Orientation.Horizontal;

                //append datagrid to each stackpanel 
                spf.Children.Add(s.gvSystem);


                xpanderS.Content = spf;
                SistemasAutos.Children.Add(xpanderS);
            }
        }

        private void CheckAtrrClass(object sender, RoutedEventArgs e)
        {
            string nombresAtributos = "";
            foreach (Sistema s in sistemasDelAuto)
            {
                foreach (Funcion f in s.funkDeSistema)
                {
                    nombresAtributos += f.nombre + ", ";
                }
            }
            MessageBox.Show(nombresAtributos);
        }

    }

    public class Funcion
    {
        public string ID { get; set; }
        public string nombre { get; set; }
        public string NAR { get; set; }
        public string RDW { get; set; }
        public string Gesetz { get; set; }
        public string KW { get; set; }
        public string Jahr { get; set; }
        public string descripcion { get; set; }
        public List<Bemerkung> bemFuncion;
        public List<Edit_Campos_Funcion> ecf;
     
        public Funcion()
        {
          
            bemFuncion = new List<Bemerkung>();
            ecf = null;
        }

        public void addBemerkungFuncion(Bemerkung b)
        {
            bemFuncion.Add(b);
        }

        public void addECFFuncion(Edit_Campos_Funcion ecf)
        {
            this.ecf.Add(ecf);
        }

    }

    public class Bemerkung
    {
        public string ID, bemerkung, funktion_ID, editado_por, evento_ID;
        
        public Bemerkung(
            string ID,
            string bemerkung,
            string funktion_ID,
            string editado_por,
            string evento_ID
        )
        {
            this.ID = ID;
            this.bemerkung = bemerkung;
            this.funktion_ID = funktion_ID;
            this.editado_por = editado_por;
            this.evento_ID = evento_ID;

        }
    }

    public class Edit_Campos_Funcion
    {
        public string ID, einsatz, abgesichert;
      
        public Edit_Campos_Funcion(
            string ID,
            string einsatz,
            string abgesichert
        )
        {
            this.ID = ID;
            this.einsatz = einsatz;
            this.abgesichert = abgesichert;
        }
    }


    public class Sistema
    {
        public int ID;
        public string nombre;
        public List<Funcion> funkDeSistema;
        public DataGrid gvSystem;
        public DataGrid gvBemerkungen;
        public DataGrid gvEditCamposFunk;

        public Sistema(
            int ID,
            string nombre
        )
        {
            this.ID = ID;
            this.nombre = nombre;
            funkDeSistema = new List<Funcion>();
            gvSystem = new DataGrid();
        }

        public void addFuncionSistema(Funcion f)
        {
            funkDeSistema.Add(f);
        }
    }

}
