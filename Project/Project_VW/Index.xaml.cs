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
    /// Lógica de interacción para Index.xaml
    /// </summary>
    public partial class Index : UserControl
    {
        DB db;
        DB db_forloops;
        DB getFunc;
        List<ComboBoxPairsBrowseAutos> cbp_browseAutos;
        List<Sistema> sistemasDelAuto;
        List<Cars> selectedCars;
        SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, (byte)92, (byte)153, (byte)214));
        Style style = new Style();
        // eventos
        List<ComboBoxPairsEvento> cbp;
        int IDEventSelected = -1;
        double widthSize = 200.0;
        

        public Index()
        {
            InitializeComponent();
            db = new DB();
            fillCars();
            cbp_browseAutos.Add(
                new ComboBoxPairsBrowseAutos("-1", "TODOS")
            );
            cbp = new List<ComboBoxPairsEvento>();
            string qry_getEventoNom = "SELECT nombre, ID FROM evento";

            db.openConn();          
            using (db.setComm(qry_getEventoNom))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    cbp.Add(new ComboBoxPairsEvento(
                       Convert.ToString(db.getReader()["nombre"]),
                       Convert.ToString(db.getReader()["ID"])
                     ));
                }

            }
            db.closeConn();

            filtroEventos.DisplayMemberPath = "nombre";
            filtroEventos.SelectedValuePath = "ID";
            filtroEventos.ItemsSource = cbp;

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

        
        // method to return any car that is needed
        // with a structure needed to show in frontend
        public Cars getCar(string idOfCar, string nameOfcar, int eventSelected)
        {
            string ID, nombre, NAR, RDW, Gesetz, B1_notasGrales;
            string B2_TCSRelevantes, B3_deadlines, descripcion, Einsatz_KWJahr;

            int registerCounter = 0;
            sistemasDelAuto = new List<Sistema>();

            List<Funcion> funktionSistemas;
            Sistema st;
            Funcion ft;
            Edit_Campos_Funcion ecf = null;


            // get systems that relate to a car
            // We will filter bemerkungen and edit campos funktion later


            // COUNT IF THERE IS SYSTEMS
            string qry_sistDeAutoCount = "SELECT COUNT(sistema.ID) AS numDeSist ";
            qry_sistDeAutoCount += "FROM sistema ";
            qry_sistDeAutoCount += "INNER JOIN rel_autos_sist ";
            qry_sistDeAutoCount += "ON sistema.ID = rel_autos_sist.sistema_ID ";
            qry_sistDeAutoCount += "WHERE autos_ID = ";
            qry_sistDeAutoCount += idOfCar;


            string qry_sistDeAuto = "SELECT sistema.ID, sistema.nombre FROM sistema ";
            qry_sistDeAuto += "INNER JOIN rel_autos_sist ";
            qry_sistDeAuto += "ON sistema.ID = rel_autos_sist.sistema_ID ";
            qry_sistDeAuto += "WHERE autos_ID = ";
            qry_sistDeAuto += idOfCar;


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
            if (registerCounter <= 0)
            {
                db.sendMBandCloseConn("No hay Sistemas relacionados a este auto: " + nameOfcar);
                return null;
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


            db.closeConn();

            #region Get funktions of each system
            // GET Funktions from each System
            foreach (Sistema ptrSistema in sistemasDelAuto)
            {
                string qry_getFunctionsCount = "SELECT COUNT(*) AS numFuncSistema ";
                qry_getFunctionsCount += "FROM funktion WHERE sistema_ID = ";
                string qry_getFunctions = "SELECT * FROM funktion WHERE sistema_ID = ";
                qry_getFunctions += ptrSistema.ID;
                qry_getFunctionsCount += ptrSistema.ID;
                db_forloops = new DB();

                db_forloops.openConn();
                // check if system has funktions
                using (db_forloops.setComm(qry_getFunctionsCount))
                {
                    db_forloops.setReader();
                    while (db_forloops.getReader().Read())
                    {
                        registerCounter = Convert.ToInt32(db_forloops.getReader()["numFuncSistema"]);
                    }
                }
                if (registerCounter <= 0)
                {
                    continue;
                }
                db_forloops.closeConn();

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
                       

                        ID = Convert.ToString(getFunc.getReader()["ID"]);
                        nombre = Convert.ToString(getFunc.getReader()["nombre"]);
                        NAR = Convert.ToString(getFunc.getReader()["NAR"]);
                        RDW = Convert.ToString(getFunc.getReader()["RDW"]);
                        Gesetz = Convert.ToString(getFunc.getReader()["Gesetz"]);
                        B1_notasGrales = Convert.ToString(getFunc.getReader()["B1_notasGrales"]);
                        B2_TCSRelevantes = Convert.ToString(getFunc.getReader()["B2_TCSRelevantes"]);
                        descripcion = Convert.ToString(getFunc.getReader()["descripcion"]);
                        Einsatz_KWJahr = Convert.ToString(getFunc.getReader()["Einsatz_KWJahr"]);
                       
                    

                        Console.WriteLine(ID);
                        ft = new Funcion()
                        {
                            ID = ID,
                            nombre = nombre,
                            NAR = NAR,
                            RDW = RDW,
                            Gesetz = Gesetz,
                            B1_notasGrales = B1_notasGrales,
                            B2_TCSRelevantes = B2_TCSRelevantes,
                            descripcion = descripcion,
                            Einsatz_KWJahr = Einsatz_KWJahr
                        };

                        // we get infromation from funktion 
                        // we have to get information from bemerkungen of
                        // this funktion and its specific information
                        // about edit_campos_funcion



                        string qry_ECFCount = "SELECT COUNT(*) as existsECF ";
                        qry_ECFCount += "FROM edit_campos_funktion ";
                        qry_ECFCount += "WHERE funktion_ID = " + ID;
                        qry_ECFCount += " AND evento_ID = " + eventSelected;
                        qry_ECFCount += " AND auto_ID = " + idOfCar;

                        string qry_ECF = "SELECT * ";
                        qry_ECF += "FROM edit_campos_funktion ";
                        qry_ECF += "WHERE funktion_ID = " + ID;
                        qry_ECF += " AND evento_ID = " + eventSelected;
                        qry_ECF += " AND auto_ID = " + idOfCar;


                        // filter edit campos funcion 
                        // by funktion id, evento,ID and auto_ID

                        // FIRST COUNT IF THE TABLE EXISTS
                        db_forloops.openConn();
                        using (db_forloops.setComm(qry_ECFCount))
                        {
                            db_forloops.setReader();
                            while (db_forloops.getReader().Read())
                            {
                                registerCounter = Convert.ToInt32(db_forloops.getReader()["existsECF"]);
                            }
                        }
                        db_forloops.closeConn();

                        // IF table does not exists, then insert table
                        // with values:
                        // in einsatz and abgesichert and edited_by ALL empty
                        // funktion_ID, the funktion that we are currently itearating
                        // evento_ID, the event that we are on
                        // and car_ID, the car id that the user has chosen.

                        if (registerCounter <= 0)
                        {
                            // close the other connection so we dont lock the db file
                            int affectedRows;
                            string insrtEditCampos = "INSERT INTO edit_campos_funktion";
                            insrtEditCampos += "(Relevant, abgesichert, B3_deadlines, ";
                            insrtEditCampos += "funktion_ID, evento_ID, auto_ID) VALUES (";
                            insrtEditCampos += " '" + "', '" + "', '" + "', ";
                            insrtEditCampos += ID + ", " + eventSelected + ", ";
                            insrtEditCampos += idOfCar + ")";

                            using (getFunc.setComm(insrtEditCampos))
                            {
                                affectedRows = getFunc.getComm().ExecuteNonQuery();
                            }
                            if (affectedRows == 0)
                            {
                                MessageBox.Show("No se pudo obtener de manera correcta la informacion de una funcion. " + "FID: " + ID);
                            }
                        }

                        db_forloops.openConn();
                        using (db_forloops.setComm(qry_ECF))
                        {
                            db_forloops.setReader();
                            while (db_forloops.getReader().Read())
                            {
                                ecf = new Edit_Campos_Funcion()
                                {
                                    ID = Convert.ToString(db_forloops.getReader()["ID"]),
                                    Relevant = Convert.ToString(db_forloops.getReader()["Relevant"]),
                                    abgesichert = Convert.ToString(db_forloops.getReader()["abgesichert"]),
                                    B3_deadlines = Convert.ToString(db_forloops.getReader()["B3_deadlines"])
                                };
                                ptrSistema.addECFFuncion(ecf);
                            }
                        }
                        db_forloops.closeConn();
                        // END information retrieval
                        ptrSistema.addFuncionSistema(ft);
                    }
                }

                // add funktions of each system to ItemsSource of GRID

                ptrSistema.gvSystem.ItemsSource = ptrSistema.funkDeSistema;

                // create the data template
                DataTemplate cardLayout = new DataTemplate();
                cardLayout.DataType = typeof(Funcion);

                #region Add Details functionality

                //set up the stack panel
                FrameworkElementFactory expander = new FrameworkElementFactory(typeof(StackPanel));
                
                FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
                spFactory.Name = "mStackFactory";
                spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                Binding bindDescripcion = new Binding()
                {
                    Path = new PropertyPath("descripcion"),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
               
                //set up the card holder textblock
                FrameworkElementFactory descripcionDetails = new FrameworkElementFactory(typeof(TextBox));
                descripcionDetails.SetBinding(TextBox.TextProperty, bindDescripcion);
                descripcionDetails.SetValue(TextBox.WidthProperty, widthSize);
                descripcionDetails.SetValue(TextBox.TextWrappingProperty, TextWrapping.Wrap);
                descripcionDetails.SetValue(TextBox.AcceptsReturnProperty, true);
                descripcionDetails.SetValue(TextBox.ToolTipProperty, "Descripcion");

                spFactory.AppendChild(descripcionDetails);

                Binding bindTCS = new Binding()
                {
                    Path = new PropertyPath("B2_TCSRelevantes"),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                //set up the card number textblock
                FrameworkElementFactory B2_TCSRelevantesDetails = new FrameworkElementFactory(typeof(TextBox));
                B2_TCSRelevantesDetails.SetBinding(TextBox.TextProperty, bindTCS);
                B2_TCSRelevantesDetails.SetValue(TextBox.WidthProperty, widthSize);
                B2_TCSRelevantesDetails.SetValue(TextBox.TextWrappingProperty, TextWrapping.Wrap);
                descripcionDetails.SetValue(TextBox.AcceptsReturnProperty, true);
                B2_TCSRelevantesDetails.SetValue(TextBox.ToolTipProperty, "Gesetz Relevantes");
                spFactory.AppendChild(B2_TCSRelevantesDetails);

                expander.AppendChild(spFactory);
                //set the visual tree of the data template
                cardLayout.VisualTree = expander;

                //set the item template to be our shiny new data template
                ptrSistema.gvSystem.RowDetailsTemplate = cardLayout;

                #endregion 

                ptrSistema.gvEditCamposFunk.ItemsSource = ptrSistema.ecf;
                // add ItemsSource to GRID of EditCamposFunkt of each system
                #endregion
                getFunc.closeConn();
            }
            #endregion
            Cars returnThisCar = new Cars(idOfCar, nameOfcar, sistemasDelAuto);
            return returnThisCar;
            // end of systems of car retrieval
            // put information in frontend
        }


        public void fAuto_dropdownClosed(object sender, EventArgs e)
        {
            // if nothing was chosen then return
            if (filtroAutos.SelectedValue == null)
                return;

            if(filtroEventos.SelectedValue == null)
            {
                IDEventSelected = SesionUsuario.getIDEvento();
            }
            else
            {
                IDEventSelected = Convert.ToInt32(filtroEventos.SelectedValue.ToString());
            }
            // HAVE to finish event dropdown handling 

            string ID_selectedCar = filtroAutos.SelectedValue.ToString();
            string selected_name = filtroAutos.Text;
            SistemasAutos.Children.Clear();
            selectedCars = new List<Cars>();
            // Have to add all cars or just one,
            // methods are already declared, have to implement them
            // there is already a list of global cars, to add as many as necessary

            if (Convert.ToInt32(ID_selectedCar) == -1)
            {
                foreach(ComboBoxPairsBrowseAutos cbp in cbp_browseAutos)
                {
                    selectedCars.Add(getCar(cbp.ID, cbp.modelo, IDEventSelected));
                }  
            }
            else
            {
                selectedCars.Add(getCar(ID_selectedCar, selected_name, IDEventSelected));
            }
            showInformationOfCar();
        }


        public void fEvent_dropdownClosed(object sender, EventArgs e)
        {
            // if nothing was chosen then return
            if (filtroEventos.SelectedValue == null)
            {
                IDEventSelected = SesionUsuario.getIDEvento();
            }
            else
            {
                IDEventSelected = Convert.ToInt32(filtroEventos.SelectedValue.ToString());
            }
            if (filtroAutos.SelectedValue == null)
                return;
            
            // HAVE to finish event dropdown handling 

            string ID_selectedCar = filtroAutos.SelectedValue.ToString();
            string selected_name = filtroAutos.Text;
            SistemasAutos.Children.Clear();
            selectedCars = new List<Cars>();
            // Have to add all cars or just one,
            // methods are already declared, have to implement them
            // there is already a list of global cars, to add as many as necessary

            if (Convert.ToInt32(ID_selectedCar) == -1)
            {
                foreach (ComboBoxPairsBrowseAutos cbp in cbp_browseAutos)
                {
                    selectedCars.Add(getCar(cbp.ID, cbp.modelo, IDEventSelected));
                }
            }
            else
            {
                selectedCars.Add(getCar(ID_selectedCar, selected_name, IDEventSelected));
            }
            showInformationOfCar();
        }

        public void showInformationOfCar()
        {            
            // SHOW infromation that was retreived from car

            foreach (Cars cars in selectedCars)
            {
                if (cars == null) continue;
                Expander xpanderC = new Expander();
                xpanderC.Background = brush;
                xpanderC.Header = cars.modelo;

                StackPanel spC = new StackPanel();
                foreach (Sistema s in cars.carSystems)
                {    
                    // check after how to hide the values of the id
                    Expander xpanderS = new Expander();
                    xpanderS.Background = Brushes.Tan;
                    xpanderS.Header = s.nombre;

                    ScrollViewer sv = new ScrollViewer();
                    sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                    StackPanel spf = new StackPanel();
                    //spf.Width = 900;
                    // stackpanel for EditCampos FUnktion
                    StackPanel spf_ECF = new StackPanel();
                    // change to horizontal to put ECF next to info of funktion
                    spf.Orientation = Orientation.Horizontal;

                    // add datagrid of edit campos funk to  spf_ECF
                    spf_ECF.Children.Add(s.gvEditCamposFunk);
       
                    //append datagrid to each stackpanel 
                    spf.Children.Add(s.gvSystem);
                    spf.Children.Add(spf_ECF);

                    sv.Content = spf;

                    xpanderS.Content = sv;
                    spC.Children.Add(xpanderS);
                }
                xpanderC.Content = spC;
                SistemasAutos.Children.Add(xpanderC);
            }

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                db.openConn();
                db.tr = db.getConn().BeginTransaction();
                foreach (Cars c in selectedCars)
                {
                    foreach (Sistema s in sistemasDelAuto)
                    {
                        foreach (Funcion f in s.funkDeSistema)
                        {
                            string updateFunk = "UPDATE funktion SET ";
                            updateFunk += "nombre = " + "'" + f.nombre  + "'" + ", ";
                            updateFunk += "NAR = " + "'" + f.NAR + "'" + ", ";
                            updateFunk += "RDW = " + "'" + f.RDW + "'" + ", ";
                            updateFunk += "Gesetz = " + "'" + f.Gesetz + "'" + ", ";
                            updateFunk += "B1_notasGrales = " + "'" + f.B1_notasGrales + "'" + ", ";
                            updateFunk += "B2_TCSRelevantes = " + "'" + f.B2_TCSRelevantes + "'" + ", ";
                            updateFunk += "descripcion = " + "'" + f.descripcion + "'" + ", ";
                            updateFunk += "Einsatz_KWJahr = " + "'" + f.Einsatz_KWJahr + "'" + " ";
                            updateFunk += "WHERE ID = " + f.ID;

                            using (db.setComm(updateFunk))
                            {
                                db.getComm().ExecuteNonQuery();
                            }
                        }
                        foreach (Edit_Campos_Funcion ecf in s.ecf)
                        {
                            string updateECF = "UPDATE edit_campos_funktion SET ";
                            updateECF += "Relevant = " + "'" + ecf.Relevant + "'" + ", ";
                            updateECF += "abgesichert = " + "'" + ecf.abgesichert + "'" + ", ";
                            updateECF += "B3_deadlines = " + "'" + ecf.B3_deadlines + "'" + " ";
                            updateECF += "WHERE ID = " + ecf.ID;

                            using (db.setComm(updateECF))
                            {
                                db.getComm().ExecuteNonQuery();
                            }
                        }
                    }
                }

                db.tr.Commit();

            } catch (SQLiteException ex)
            {
                MessageBox.Show("Error: {0}", ex.ToString());

                if (db.tr != null)
                {
                    try
                    {
                        db.tr.Rollback();
                    }
                    catch (SQLiteException ex2)
                    {

                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());

                    }
                    finally
                    {
                        db.tr.Dispose();
                    }
                }
            }
            finally
            {
                if (db.getComm() != null)
                {
                    db.getComm().Dispose();
                }

                if (db.tr != null)
                {
                    db.tr.Dispose();
                }

                if (db.getConn() != null)
                {
                    try
                    {
                        db.getConn().Close();

                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        db.getConn().Dispose();
                    }
                }
            }
            MessageBox.Show("Información actualizada correctamente");
        }

        private void hideCols(object sender, RoutedEventArgs e)
        {
            foreach (Cars c in selectedCars)
            {
                foreach (Sistema s in sistemasDelAuto)
                {
                    
                    if (s.gvSystem.Columns.Count > 0)
                    {
                        foreach (DataGridColumn column in s.gvSystem.Columns)
                        {
                            column.Width = new DataGridLength(120);
                        }
                        foreach (DataGridColumn column in s.gvEditCamposFunk.Columns)
                        {
                            column.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader);
                        }
                       
                        s.gvSystem.Columns[0].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[2].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[6].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[7].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[8].Visibility = Visibility.Collapsed;

                        s.gvEditCamposFunk.Columns[0].Visibility = Visibility.Collapsed;
                    }                   
                }
            }
        }

        private void showCols(object sender, RoutedEventArgs e)
        {
            foreach (Cars c in selectedCars)
            {
                foreach (Sistema s in sistemasDelAuto)
                {
                    if (s.gvSystem.Columns.Count > 0)
                    {
                        s.gvSystem.Columns[2].Visibility = Visibility.Visible;
                        s.gvSystem.Columns[6].Visibility = Visibility.Visible;
                        s.gvSystem.Columns[7].Visibility = Visibility.Visible;
                        s.gvSystem.Columns[8].Visibility = Visibility.Visible;                     
                    }
                }
                        
            }
        }
    }

    public class Funcion
    {
        public string ID { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string NAR { get; set; }
        public string RDW { get; set; }
        public string Gesetz { get; set; }
        public string B1_notasGrales { get; set; }
        public string B2_TCSRelevantes { get; set; }
        public string Einsatz_KWJahr { get; set; }
        public string color { get; set; }
    }


    public class Edit_Campos_Funcion
    {
        public string ID { get; set; }
        public string Relevant { get; set; }
        public string abgesichert { get; set; }
        public string B3_deadlines { get; set; }
    }

    public class Cars
    {
        public string ID, modelo;
        public List<Sistema> carSystems;
        public Cars(string ID, string modelo, List<Sistema> carSystems)
        {
            this.ID = ID;
            this.modelo = modelo;
            this.carSystems = carSystems;
        }
    }

    public class Sistema
    {
        public int ID;
        public string nombre;
        public List<Funcion> funkDeSistema;
        public List<Edit_Campos_Funcion> ecf;
        public DataGrid gvSystem;
        public DataGrid gvEditCamposFunk;

        public Sistema(
            int ID,
            string nombre
        )
        {
            this.ID = ID;
            this.nombre = nombre;
            funkDeSistema = new List<Funcion>();
            ecf = new List<Edit_Campos_Funcion>();
            gvSystem = new DataGrid();
            gvEditCamposFunk = new DataGrid();

        }

        public void addFuncionSistema(Funcion f)
        {
            funkDeSistema.Add(f);
        }

        public void addECFFuncion(Edit_Campos_Funcion ecf)
        {
            this.ecf.Add(ecf);
        }
    }
}
