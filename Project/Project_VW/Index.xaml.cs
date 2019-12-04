using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
using OfficeOpenXml;


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
        List<CheckBoxPairsSistemas> cbpairs;
        List<ComboBoxPairsBrowseAutos> cbp_browseAutos;
        List<Sistema> sistemasDelAuto;
        List<Cars> selectedCars;
        SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, (byte)92, (byte)153, (byte)214));
        SolidColorBrush brushBlue = new SolidColorBrush(Color.FromArgb(255, (byte)13, (byte)70, (byte)113));
        SolidColorBrush brushRed = new SolidColorBrush(Color.FromArgb(255, (byte)220, (byte)53, (byte)34));
        SolidColorBrush brushYellow = new SolidColorBrush(Color.FromArgb(255, (byte)241, (byte)203, (byte)98));
        Style style = new Style();
        // eventos
        List<ComboBoxPairsEvento> cbp;
        int IDEventSelected = 1;
        double widthSize = 200.0;

        int selectedColor = 0;

        #region to change color value of each grid 
        Binding bindColor = new Binding()
        {
            Path = new PropertyPath("Ampel"),
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        
        Dictionary<int, string> colores = new Dictionary<int, string>();
        #endregion

        public Index()
        {
            //ADD Colors to dictionary of color
            this.colores.Add(1, "Rot");
            this.colores.Add(2, "Gelb");
            this.colores.Add(3, "Keine");

            InitializeComponent();

            cbpairs = new List<CheckBoxPairsSistemas>();
            cbpairs.Add(new CheckBoxPairsSistemas("1", "Rot"));
            cbpairs.Add(new CheckBoxPairsSistemas("2", "Gelb"));
            cbpairs.Add(new CheckBoxPairsSistemas("3", "Alle"));

            filtroColor.DisplayMemberPath = "nombre";
            filtroColor.SelectedValuePath = "ID";
            filtroColor.ItemsSource = cbpairs;

            selectedCars = new List<Cars>();
            db = new DB();
            fillCars();
            cbp_browseAutos.Add(
                new ComboBoxPairsBrowseAutos("-1", "TODOS")
            );
              
        }

        

        public void fillCars()
        {
            cbp_browseAutos = new List<ComboBoxPairsBrowseAutos>();
            string qry_getEventos = "SELECT ID, modelo FROM autos WHERE isActive = 1";
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
        public Cars getCar(string idOfCar, string nameOfcar, int eventSelected, int color)
        {
            string ID, Funktion, NAR, RDW, Gesetz, Bemerkungen1;
            string Gesetz_TF, Beschreibung, Einsatz_KWJahr;
            string ID_ECF = "", Relevant = "", Abgesichert = "", Ampel = "", Absicherung_Termin = "", Bemerkungen2 = "";

            int registerCounter = 0;
            sistemasDelAuto = new List<Sistema>();

            List<Funcion> funktionSistemas;
            Sistema st;
            Funcion ft;

            string qry_sistDeAutoCount = "";
            string qry_sistDeAuto = "";
            // get systems that relate to a car
            // We will filter bemerkungen and edit campos funktion later

            // COUNT IF THERE IS SYSTEMS
            if (color == 3)
            {
                qry_sistDeAutoCount = "SELECT COUNT(sistema.ID) AS numDeSist ";
                qry_sistDeAutoCount += "FROM sistema ";
                qry_sistDeAutoCount += "INNER JOIN rel_autos_sist ";
                qry_sistDeAutoCount += "ON sistema.ID = rel_autos_sist.sistema_ID AND sistema.isActive = 1 ";
                qry_sistDeAutoCount += "WHERE autos_ID = ";
                qry_sistDeAutoCount += idOfCar;

                qry_sistDeAuto = "SELECT sistema.ID, sistema.nombre FROM sistema ";
                qry_sistDeAuto += "INNER JOIN rel_autos_sist ";
                qry_sistDeAuto += "ON sistema.ID = rel_autos_sist.sistema_ID AND sistema.isActive = 1 ";
                qry_sistDeAuto += "WHERE autos_ID = ";
                qry_sistDeAuto += idOfCar;
            }
            else
            {

                qry_sistDeAutoCount = "SELECT COUNT(sistema.ID) AS numDeSist FROM sistema ";
                qry_sistDeAutoCount += "INNER JOIN funktion ON sistema.ID = funktion.sistema_ID ";
                qry_sistDeAutoCount += "WHERE funktion.ID IN (";
                qry_sistDeAutoCount += "SELECT funktion.ID FROM funktion INNER JOIN ";
                qry_sistDeAutoCount += "edit_campos_funktion ON edit_campos_funktion.funktion_ID = funktion.ID ";
                qry_sistDeAutoCount += "WHERE edit_campos_funktion.Ampel = " + color + " AND edit_campos_funktion.auto_ID = " + idOfCar;
                qry_sistDeAutoCount += ")";

                qry_sistDeAuto = "SELECT DISTINCT sistema.ID, sistema.nombre FROM sistema ";
                qry_sistDeAuto += "INNER JOIN funktion ON sistema.ID = funktion.sistema_ID ";
                qry_sistDeAuto += "WHERE funktion.ID IN (";
                qry_sistDeAuto += "SELECT funktion.ID FROM funktion INNER JOIN ";
                qry_sistDeAuto += "edit_campos_funktion ON edit_campos_funktion.funktion_ID = funktion.ID ";
                qry_sistDeAuto += "WHERE edit_campos_funktion.Ampel = " + color + " AND edit_campos_funktion.auto_ID = " + idOfCar;
                qry_sistDeAuto += ")";
            }


           

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

            #region Get Systems of CAR
            // GET Funktions from each System
            foreach (Sistema ptrSistema in sistemasDelAuto)
            {
                string qry_getFunctionsCount = "";
                string qry_getFunctions = "";
              
                // IF no color was selected
                if (color == 3)
                {
                    qry_getFunctionsCount = "SELECT COUNT(*) AS numFuncSistema ";
                    qry_getFunctionsCount += "FROM funktion WHERE sistema_ID = ";
                    qry_getFunctionsCount += ptrSistema.ID;
                    qry_getFunctionsCount += " AND isActive = 1";

                    qry_getFunctions = "SELECT * FROM funktion WHERE sistema_ID = ";
                    qry_getFunctions += ptrSistema.ID;
                    qry_getFunctions += " AND isActive = 1";
                } 
                else // select only registers that correspond to the specific color
                {
                    /*
                    qry_getFunctionsCount = "SELECT COUNT(*) AS numFuncSistema ";
                    qry_getFunctionsCount += "FROM funktion INNER JOIN edit_campos_funktion " +
                        "ON funktion.ID = edit_campos_funktion.funktion_ID AND Ampel = " + color + " " +
                        "WHERE sistema_ID = ";
                    qry_getFunctionsCount += ptrSistema.ID;
                    qry_getFunctionsCount += " AND isActive = 1";
                    */


                    qry_getFunctions = "SELECT * FROM funktion INNER JOIN edit_campos_funktion ";
                    qry_getFunctions += "ON funktion.ID = edit_campos_funktion.funktion_ID ";
                    qry_getFunctions += "AND edit_campos_funktion.Ampel = " + color + " ";
                    qry_getFunctions += "AND edit_campos_funktion.auto_ID = " + idOfCar + " ";
                    qry_getFunctions += "WHERE sistema_ID = " + ptrSistema.ID + " ";
                    qry_getFunctions += "AND isActive = 1";

                   
                }
                

                db_forloops = new DB();

                if(color == 3)
                {
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
              
                        ID = Convert.ToString(getFunc.getReader()["ID"]);
                        Funktion = Convert.ToString(getFunc.getReader()["Funktion"]);
                        NAR = Convert.ToString(getFunc.getReader()["NAR"]);
                        RDW = Convert.ToString(getFunc.getReader()["RDW"]);
                        Gesetz = Convert.ToString(getFunc.getReader()["Gesetz"]);
                        Bemerkungen1 = Convert.ToString(getFunc.getReader()["Bemerkungen1"]);
                        Gesetz_TF = Convert.ToString(getFunc.getReader()["Gesetz_TF"]);
                        Beschreibung = Convert.ToString(getFunc.getReader()["Beschreibung"]);
                        Einsatz_KWJahr = Convert.ToString(getFunc.getReader()["Einsatz_KWJahr"]);
                       
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
                            insrtEditCampos += "(Relevant, Abgesichert, Absicherung_Termin, ";
                            insrtEditCampos += "funktion_ID, evento_ID, auto_ID, Ampel, Bemerkungen2) VALUES (";
                            insrtEditCampos += " '" + "', '" + "', '" + "', ";
                            insrtEditCampos += ID + ", " + eventSelected + ", ";
                            insrtEditCampos += idOfCar + ", 3, '" + "')";

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

                                ID_ECF = Convert.ToString(db_forloops.getReader()["ID"]);
                                Relevant = Convert.ToString(db_forloops.getReader()["Relevant"]);
                                Abgesichert = Convert.ToString(db_forloops.getReader()["abgesichert"]);
                                Absicherung_Termin = Convert.ToString(db_forloops.getReader()["Absicherung_Termin"]);
                                Ampel = Convert.ToString(db_forloops.getReader()["Ampel"]);
                                Bemerkungen2 = Convert.ToString(db_forloops.getReader()["Bemerkungen2"]);
                            }
                        }

                        // create funktion object  to append to system
                        ft = new Funcion()
                        {
                            ID = ID,
                            Funktion = Funktion,
                            NAR = NAR,
                            RDW = RDW,
                            Gesetz = Gesetz,
                            Bemerkungen1 = Bemerkungen1,
                            Gesetz_TF = Gesetz_TF,
                            Beschreibung = Beschreibung,
                            Einsatz_KWJahr = Einsatz_KWJahr,
                            ID_ECF = ID_ECF,
                            Relevant = Relevant,
                            Abgesichert = Abgesichert,
                            Absicherung_Termin = Absicherung_Termin,
                            Ampel = Ampel,
                            Bemerkungen2 = Bemerkungen2
                        };

                        db_forloops.closeConn();
                        // END information retrieval
                        ptrSistema.addFuncionSistema(ft);
                    }
                }

                // add funktions of each system to ItemsSource of GRID

               
                DataGridComboBoxColumn comboBoxColumn = new DataGridComboBoxColumn();

                comboBoxColumn.Header = "Ampel";
                comboBoxColumn.SelectedValuePath = "Key";
                comboBoxColumn.DisplayMemberPath = "Value";
                comboBoxColumn.ItemsSource = colores;
                comboBoxColumn.SelectedValueBinding = bindColor;

                ptrSistema.gvSystem.ItemsSource = ptrSistema.funkDeSistema;
                ptrSistema.gvSystem.CanUserAddRows = false;
                ptrSistema.gvSystem.CanUserDeleteRows = false;
                ptrSistema.gvSystem.CanUserSortColumns = false;
                ptrSistema.gvSystem.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;

                #region COLOR ROWS 
                // style to append color triggers
                Style style = new Style();

                style.TargetType = typeof(DataGridRow);

                #region Trigger to color red
                // setter for foreground color
                Setter sttForeground = new Setter()
                {
                    Property = ForegroundProperty,
                    Value = Brushes.White
                };

                Setter stt = new Setter()
                {
                    Property = DataGridRow.BackgroundProperty,
                    Value = brushRed
                };
                // new bind for property color
                Binding bindC = new Binding()
                {
                    Path = new PropertyPath("Ampel")
                };

                DataTrigger dt = new DataTrigger()
                {
                    Binding = bindC,
                    Value = "1"
                };

                #endregion

                #region Trigger to color yellow

                Setter sttY = new Setter()
                {
                    Property = DataGridRow.BackgroundProperty,
                    Value = brushYellow
                };

                Binding bindCY = new Binding()
                {
                    Path = new PropertyPath("Ampel")
                };

                DataTrigger dtY = new DataTrigger()
                {
                    Binding = bindCY,
                    Value = "2"
                };

                #endregion

                #endregion
                // create the data template

                DataTemplate cardLayout = new DataTemplate();
                cardLayout.DataType = typeof(Funcion);

                #region Add Details functionality

                Binding bindDescripcion = new Binding()
                {
                    Path = new PropertyPath("Beschreibung"),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
               
                //set up the card holder textblock
                FrameworkElementFactory descripcionDetails = new FrameworkElementFactory(typeof(TextBox));
                descripcionDetails.SetBinding(TextBox.TextProperty, bindDescripcion);
                descripcionDetails.SetValue(TextBox.WidthProperty, widthSize);
                descripcionDetails.SetValue(TextBox.TextWrappingProperty, TextWrapping.Wrap);
                descripcionDetails.SetValue(TextBox.AcceptsReturnProperty, true);
                descripcionDetails.SetValue(TextBox.ToolTipProperty, "Beschreibung");

                //set the visual tree of the data template
                cardLayout.VisualTree = descripcionDetails;

                //set the item template to be our shiny new data template
                ptrSistema.gvSystem.RowDetailsTemplate = cardLayout;
                #endregion

                ptrSistema.gvSystem.Columns.Add(comboBoxColumn);


                // add setters to datatrgiggers
                dt.Setters.Add(stt);
                dt.Setters.Add(sttForeground);

                dtY.Setters.Add(sttY);

                style.Triggers.Clear();
                style.Triggers.Add(dt);
                style.Triggers.Add(dtY);
                ptrSistema.gvSystem.RowStyle = style;
             
              
                #endregion
                getFunc.closeConn();
            }
            #endregion
            Cars returnThisCar = new Cars(idOfCar, nameOfcar, sistemasDelAuto);
            return returnThisCar;
            // end of systems of car retrieval
            // put information in frontend
        }
      
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        public void fColor_dropdownClosed(object sender, EventArgs e)
        {
            if (filtroAutos.SelectedValue == null )
            {
                if (filtroColor.SelectedValue == null) return;
                selectedColor = Convert.ToInt32(filtroColor.SelectedValue.ToString());
            }          
            // if car is checked then have to paint again all from this car.
            else
            {
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
                        if (filtroColor.SelectedValue == null)
                            selectedCars.Add(getCar(cbp.ID, cbp.modelo, IDEventSelected, 3));
                        else
                            selectedCars.Add(getCar(cbp.ID, cbp.modelo, IDEventSelected, Convert.ToInt32(filtroColor.SelectedValue.ToString())));
                    }
                }
                else
                {
                    if (filtroColor.SelectedValue == null)
                        selectedCars.Add(getCar(ID_selectedCar, selected_name, IDEventSelected, 3));
                    else
                        selectedCars.Add(getCar(ID_selectedCar, selected_name, IDEventSelected, Convert.ToInt32(filtroColor.SelectedValue.ToString())));
                }
                showInformationOfCar();
            }
        }

        public void fAuto_dropdownClosed(object sender, EventArgs e)
        {
            // if nothing was chosen then return
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
                foreach(ComboBoxPairsBrowseAutos cbp in cbp_browseAutos)
                {
                    if (filtroColor.SelectedValue == null)
                        selectedCars.Add(getCar(cbp.ID, cbp.modelo, IDEventSelected, 3));
                    else
                        selectedCars.Add(getCar(cbp.ID, cbp.modelo, IDEventSelected, Convert.ToInt32(filtroColor.SelectedValue.ToString())));
                }  
            }
            else
            {
                if (filtroColor.SelectedValue == null)
                    selectedCars.Add(getCar(ID_selectedCar, selected_name, IDEventSelected, 3));
                else
                    selectedCars.Add(getCar(ID_selectedCar, selected_name, IDEventSelected, Convert.ToInt32(filtroColor.SelectedValue.ToString())));
            }
            showInformationOfCar();
        }

        /*
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

                    // IF EVENT WAS CHANGED

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
                            if (filtroColor.SelectedValue == null)
                                selectedCars.Add(getCar(cbp.ID, cbp.modelo, IDEventSelected, 0));
                            else
                                selectedCars.Add(getCar(cbp.ID, cbp.modelo, IDEventSelected, Convert.ToInt32(filtroColor.SelectedValue.ToString())));
                        }
                    }
                    else
                    {
                        if (filtroColor.SelectedValue == null)
                            selectedCars.Add(getCar(ID_selectedCar, selected_name, IDEventSelected, 3));
                        else
                            selectedCars.Add(getCar(ID_selectedCar, selected_name, IDEventSelected, Convert.ToInt32(filtroColor.SelectedValue.ToString())));
                    }

                    showInformationOfCar();
                    // END OF CHANGED EVENT
                }
                */

        public void showInformationOfCar()
        {
            // SHOW infromation that was retreived from car
            foreach (Cars cars in selectedCars)
            {
                if (cars == null) continue;
                Expander xpanderC = new Expander();
                xpanderC.IsExpanded = true;
                xpanderC.Background = brush;
                xpanderC.Header = cars.modelo;

                StackPanel spC = new StackPanel();
                foreach (Sistema s in cars.carSystems)
                {
                    Expander xpanderS = new Expander();
                    xpanderS.Background = brushBlue;
                    xpanderS.Foreground = Brushes.White;
                    xpanderS.Header = s.nombre;
                    xpanderS.Content = s.gvSystem;
                    spC.Children.Add(xpanderS);
                }
                xpanderC.Content = spC;
                SistemasAutos.Children.Add(xpanderC);
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (selectedCars.Count < 1) return;
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
                            updateFunk += "Funktion = " + "'" + f.Funktion  + "'" + ", ";
                            updateFunk += "NAR = " + "'" + f.NAR + "'" + ", ";
                            updateFunk += "RDW = " + "'" + f.RDW + "'" + ", ";
                            updateFunk += "Gesetz = " + "'" + f.Gesetz + "'" + ", ";
                            updateFunk += "Bemerkungen1 = " + "'" + f.Bemerkungen1 + "'" + ", ";
                            updateFunk += "Gesetz_TF = " + "'" + f.Gesetz_TF + "'" + ", ";
                            updateFunk += "Beschreibung = " + "'" + f.Beschreibung + "'" + ", ";
                            updateFunk += "Einsatz_KWJahr = " + "'" + f.Einsatz_KWJahr + "'" + " ";
                            updateFunk += "WHERE ID = " + f.ID;

                            using (db.setComm(updateFunk))
                            {
                                db.getComm().ExecuteNonQuery();
                            }

                            string updateECF = "UPDATE edit_campos_funktion SET ";
                            updateECF += "Relevant = " + "'" + f.Relevant + "'" + ", ";
                            updateECF += "Abgesichert = " + "'" + f.Abgesichert + "'" + ", ";
                            updateECF += "Absicherung_Termin = " + "'" + f.Absicherung_Termin + "'" + ", ";
                            updateECF += "Ampel= " + "'" + f.Ampel + "'" + ", ";
                            updateECF += "Bemerkungen2 = " + "'" + f.Bemerkungen2 + "'" + " ";
                            updateECF += "WHERE ID = " + f.ID_ECF;

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

        private void exportExcel(object sender, RoutedEventArgs e)
        {
            if (selectedCars.Count < 1) return;

            using(ExcelPackage excel = new ExcelPackage())
            {
                var workSheets = excel.Workbook.Worksheets.Add("WS_Prueba1");

                foreach (Cars c in selectedCars)
                {
                    workSheets.Cells["A1"].Value = c.modelo;
                }

                FileInfo excelFile = new FileInfo(@"C:\Users\VAS6150A\Desktop\text.xlsx");
                excel.SaveAs(excelFile);
            }

           

            /*
                foreach (Cars c in selectedCars)
                {
                    foreach (Sistema s in sistemasDelAuto)
                    {
                        if (s.gvSystem.Columns.Count > 0)
                        {
                            foreach (DataGridColumn column in s.gvSystem.Columns)
                            {
                                column.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader);
                            }
                            s.gvSystem.Columns[1].Visibility = Visibility.Collapsed;
                            s.gvSystem.Columns[6].Visibility = Visibility.Collapsed;
                            s.gvSystem.Columns[7].Visibility = Visibility.Collapsed;
                            s.gvSystem.Columns[8].Visibility = Visibility.Collapsed;
                            s.gvSystem.Columns[9].Visibility = Visibility.Collapsed;
                            s.gvSystem.Columns[10].Visibility = Visibility.Collapsed;
                            s.gvSystem.Columns[11].Visibility = Visibility.Collapsed;
                        }
                    }
                }
                */
        }

        private void hideCols(object sender, RoutedEventArgs e)
        {
            if (selectedCars.Count < 1) return;
            foreach (Cars c in selectedCars)
            {
                foreach (Sistema s in sistemasDelAuto)
                {
                    
                    if (s.gvSystem.Columns.Count > 0)
                    {
                        foreach (DataGridColumn column in s.gvSystem.Columns)
                        {
                            column.Width = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader);
                        } 
                        s.gvSystem.Columns[1].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[6].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[7].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[8].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[9].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[10].Visibility = Visibility.Collapsed;
                        s.gvSystem.Columns[11].Visibility = Visibility.Collapsed;
                    }                   
                }
            }
        }

        private void showCols(object sender, RoutedEventArgs e)
        {
            if (selectedCars.Count < 1) return;
            foreach (Cars c in selectedCars)
            {
                foreach (Sistema s in sistemasDelAuto)
                {
                    if (s.gvSystem.Columns.Count > 0)
                    {
                        s.gvSystem.Columns[6].Visibility = Visibility.Visible;
                        s.gvSystem.Columns[7].Visibility = Visibility.Visible;
                        s.gvSystem.Columns[8].Visibility = Visibility.Visible;
                        s.gvSystem.Columns[9].Visibility = Visibility.Visible;                            
                    }
                }
                        
            }
        }
    }

    public class Funcion
    {
        public string ID { get; set; }
        public string Funktion { get; set; }
        public string NAR { get; set; }
        public string RDW { get; set; }
        public string Gesetz { get; set; }
        public string Bemerkungen1 { get; set; }
        public string Gesetz_TF { get; set; }
        public string Beschreibung   { get; set; }
        public string Einsatz_KWJahr { get; set; }

        // fields of Edit_Campos_Funcion
        public string ID_ECF { get; set; }
        public string Ampel { get; set; }
        public string Relevant { get; set; }
        public string Abgesichert { get; set; }
        public string Absicherung_Termin { get; set; }
        public string Bemerkungen2 { get; set; }

    }



    public class Cars
    {
        public string ID, modelo, color;
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
        public DataGrid gvSystem;

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
