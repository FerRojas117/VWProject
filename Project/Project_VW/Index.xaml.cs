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
        List<ComboBoxPairsBrowseAutos> cbp_browseAutos;
        List<Sistema> sistemasDelAuto;
        public Index()
        {
            InitializeComponent();
            db = new DB();
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
            Edit_Campos_Funcion ecf;

            int ID_func;

            // get systems that relate to a car
            // We will filter bemerkungen and edit campos funktion later


            // COUNT 
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


            // GET Funktions from each System
            foreach (Sistema ptrSistema in sistemasDelAuto)
            {
                string qry_getFunctionsCount = "SELECT COUNT(*) AS numFuncSistema ";
                qry_getFunctionsCount += "FROM funktion WHERE sistema_ID = ";
                string qry_getFunctions = "SELECT * FROM funktion WHERE sistema_ID = ";
                qry_getFunctions += ptrSistema.ID;

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

                // let's look for its information, its bemerkungen and its
                // einsatz and abgesichert fields
                using (db.setComm(qry_getFunctions))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        bemerkungsFunktion = new List<Bemerkung>();

                        ID = Convert.ToString(db.getReader()["FID"]);
                        nombre = Convert.ToString(db.getReader()["FN"]);
                        NAR = Convert.ToString(db.getReader()["FNAR"]);
                        RDW = Convert.ToString(db.getReader()["FRDW"]);
                        Gesetz = Convert.ToString(db.getReader()["FG"]);
                        KW = Convert.ToString(db.getReader()["FKW"]);
                        Jahr = Convert.ToString(db.getReader()["FJ"]);
                        descripcion = Convert.ToString(db.getReader()["FD"]);

                        // we get infromation from funktion 
                        // we have to get information from bemerkungen from
                        // this funktion and its specific information
                        // about edit_campos_funcion


                    }
                }

            }
            
            
            
            using (db.setComm(qry_getFunctions))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    
                    bemerkungsFunktion = new List<Bemerkung>();

                    ID = Convert.ToString(db.getReader()["FID"]);
                    nombre = Convert.ToString(db.getReader()["FN"]);
                    NAR = Convert.ToString(db.getReader()["FNAR"]);
                    RDW = Convert.ToString(db.getReader()["FRDW"]);
                    Gesetz = Convert.ToString(db.getReader()["FG"]);
                    KW = Convert.ToString(db.getReader()["FKW"]);
                    Jahr = Convert.ToString(db.getReader()["FJ"]);
                    descripcion = Convert.ToString(db.getReader()["FD"]);
                    // GET BEMERKUNGEN FROM THIS FUNCTION
                    int numBemerkungen = 0;
                    int numEditCamposF = 0;

                    string qry_bmrkng = "SELECT * FROM bemerkung WHERE funktion_ID = ";
                    qry_bmrkng += ID;
                    string qry_bmrkngCount = "SELECT COUNT(*) as num FROM bemerkung WHERE funktion_ID = ";
                    qry_bmrkngCount += ID;

                    // READ ALL OF THEM 
                    using (db.setComm(qry_bmrkngCount))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            numBemerkungen = Convert.ToInt32(db.getReader()["num"]);
                        }
                    }

                    if(numBemerkungen > 0)
                    {
                        using (db.setComm(qry_bmrkngCount))
                        {
                            db.setReader();
                            while (db.getReader().Read())
                            {
                                Bemerkung bmr = new Bemerkung(
                                   Convert.ToString(db.getReader()["ID"]),
                                   Convert.ToString(db.getReader()["bemerkung"]),
                                   Convert.ToString(db.getReader()["funktion_ID"]),
                                   Convert.ToString(db.getReader()["editado_por"]),
                                   Convert.ToString(db.getReader()["evento_ID"])
                               );
                                bemerkungsFunktion.Add(bmr);
                            }
                        } 
                    }

                    // GET einsatz AND abgesichert
                    string qry_getEFCFunkCount = "SELECT COUNT(*) AS num FROM ";
                    qry_getEFCFunkCount += "edit_campos_funktion WHERE ";
                    qry_getEFCFunkCount += "funktion_ID = ";
                    qry_getEFCFunkCount += ID;
                    qry_getEFCFunkCount += " AND evento_ID = ";
                    qry_getEFCFunkCount += SesionUsuario.getIDEvento();
                    qry_getEFCFunkCount += " AND auto_ID = ";
                    qry_getEFCFunkCount += ID_selectedCar;

                    
                    string qry_getEFCFunk = "SELECT einsatz, abgesichert FROM ";
                    qry_getEFCFunk += "edit_campos_funktion WHERE ";
                    qry_getEFCFunk += "funktion_ID = ";
                    qry_getEFCFunk += ID;
                    qry_getEFCFunk += " AND evento_ID = ";
                    qry_getEFCFunk += SesionUsuario.getIDEvento();
                    qry_getEFCFunk += " AND auto_ID = ";
                    qry_getEFCFunk += ID_selectedCar;
                    

                    using (db.setComm(qry_getEFCFunkCount))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            numEditCamposF = Convert.ToInt32(db.getReader()["num"]);
                        }
                    }

                    // IF it exists table of edit campos funcion then append 
                    //instance to object function
                    if(numEditCamposF > 0)
                    {
                        using (db.setComm(qry_getEFCFunk))
                        {
                            db.setReader();
                            while (db.getReader().Read())
                            {
                                einsatz = Convert.ToString(db.getReader()["einsatz"]);
                                abgesichert = Convert.ToString(db.getReader()["abgesichert"]);
                            }
                        }
                    }
                }
            }
        }
    }

    public class Funcion
    {
        public string ID, nombre, NAR, RDW, Gesetz, KW, Jahr, descripcion;
        public List<Bemerkung> bemFuncion;
        public Edit_Campos_Funcion ecf;
        public Funcion(
            string ID,
            string nombre,
            string NAR,
            string RDW,
            string Gesetz,
            string KW,
            string Jahr,
            string descripcion
        )
        {
            this.ID = ID;
            this.nombre = nombre;
            this.NAR = NAR;
            this.RDW = RDW;
            this.Gesetz = Gesetz;
            this.KW = KW;
            this.Jahr = Jahr;
            this.descripcion = descripcion;
            bemFuncion = new List<Bemerkung>();
            ecf = null;
        }

        public void addBemerkungFuncion(Bemerkung b)
        {
            bemFuncion.Add(b);
        }

        public void addECFFuncion(Edit_Campos_Funcion ecf)
        {
            this.ecf = ecf;
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
        public string einsatz, abgesichert;
        public bool exists;

        public Edit_Campos_Funcion(
            string einsatz,
            string abgesichert,
            bool exists
        )
        {
            this.einsatz = einsatz;
            this.abgesichert = abgesichert;
            this.exists = exists;
        }
    }


    public class Sistema
    {
        public int ID;
        public string nombre;
        public List<Funcion> funkDeSistema;


        public Sistema(
            int ID,
            string nombre
        )
        {
            this.ID = ID;
            this.nombre = nombre;
            funkDeSistema = new List<Funcion>();
        }

        public void addFuncionSistema(Funcion f)
        {
            funkDeSistema.Add(f);
        }
    }

}
