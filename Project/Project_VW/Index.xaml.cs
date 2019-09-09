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
        List<Funcion> funcionesDelSist;
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
            if (filtroAutos.SelectedValue == null)
                return;
            string ID_selectedCar = filtroAutos.SelectedValue.ToString();
            SistemasAutos.Children.Clear();

            // get funktions from car and event selected
            string qry_getFunctions = "SELECT * FROM funktion ";
            qry_getFunctions += "INNER JOIN rel_autos_sist ";
            qry_getFunctions += "ON rel_autos_sist.sistema_ID = funktion.sistema_ID ";
            qry_getFunctions += "WHERE rel_autos_sist.autos_ID = ";
            qry_getFunctions += ID_selectedCar;

            
            // funktion_ID = 1 AND evento_ID = 6 AND auto_ID = 18; "

             string ID, nombre, NAR, RDW, Gesetz, KW, Jahr, descripcion;
             string einsatz, abgesichert;
             List<Bemerkung> bemerkungsFunktion;

            using (db.setComm(qry_getFunctions))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    bemerkungsFunktion = new List<Bemerkung>();
                    ID = Convert.ToString(db.getReader()["ID"]);
                    nombre = Convert.ToString(db.getReader()["nombre"]);
                    NAR = Convert.ToString(db.getReader()["NAR"]);
                    RDW = Convert.ToString(db.getReader()["RDW"]);
                    Gesetz = Convert.ToString(db.getReader()["Gesetz"]);
                    KW = Convert.ToString(db.getReader()["KW"]);
                    Jahr = Convert.ToString(db.getReader()["Jahr"]);
                    descripcion = Convert.ToString(db.getReader()["descripcion"]);

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
                    string qry_getBemerkFunkCount = "SELECT COUNT(*) AS num FROM ";
                    qry_getBemerkFunkCount += "edit_campos_funktion WHERE ";
                    qry_getBemerkFunkCount += "funktion_ID = ";
                    qry_getBemerkFunkCount += ID;
                    qry_getBemerkFunkCount += " AND evento_ID = ";
                    qry_getBemerkFunkCount += SesionUsuario.getIDEvento();
                    qry_getBemerkFunkCount += " AND auto_ID = ";
                    qry_getBemerkFunkCount += ID_selectedCar;

                    
                    string qry_getBemerkFunk = "SELECT einsatz, abgesichert FROM ";
                    qry_getBemerkFunk += "edit_campos_funktion WHERE ";
                    qry_getBemerkFunk += "funktion_ID = ";
                    qry_getBemerkFunk += ID;
                    qry_getBemerkFunk += " AND evento_ID = ";
                    qry_getBemerkFunk += SesionUsuario.getIDEvento();
                    qry_getBemerkFunk += " AND auto_ID = ";
                    qry_getBemerkFunk += ID_selectedCar;
                    

                    using (db.setComm(qry_getBemerkFunk))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            numEditCamposF = Convert.ToInt32(db.getReader()["num"]);
                        }
                    }
                    // IF exists we have to update, 
                    if(numEditCamposF > 0)
                    {

                    }
                }
            }
        }
    }

    public class Funcion
    {
        public string ID, nombre, NAR, RDW, Gesetz, KW, Jahr, descripcion;
        public List<Bemerkung> bemerkungen;
        public Funcion()
        {
           
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


}
