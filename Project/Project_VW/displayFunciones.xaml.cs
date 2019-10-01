﻿using System;
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
        string qry_getF = "SELECT * FROM funktion";
        List<Funcion> funktions = new List<Funcion>();
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
                    funktions.Add(new Funcion()
                    {
                        ID = Convert.ToString(db.getReader()["ID"]),
                        nombre = Convert.ToString(db.getReader()["nombre"]),
                        NAR = Convert.ToString(db.getReader()["NAR"]),
                        RDW = Convert.ToString(db.getReader()["RDW"]),
                        Gesetz = Convert.ToString(db.getReader()["Gesetz"]),
                        B1_notasGrales = Convert.ToString(db.getReader()["B1_notasGrales"]),
                        B2_TCSRelevantes = Convert.ToString(db.getReader()["B2_TCSRelevantes"]),
                        descripcion = Convert.ToString(db.getReader()["descripcion"]),
                        Einsatz_KWJahr = Convert.ToString(db.getReader()["Einsatz_KWJahr"])
                    }); 
                }
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
         
        }

        private void CheckAtrrClass(object sender, RoutedEventArgs e)
        {

            
        }

        public void createDataGrid()
        {
            Style st = FindResource("popCell") as Style;
            StackPanel sp_btnBemerkungen = new StackPanel();
            sp_btnBemerkungen.Margin = new Thickness(5, 45, 0, 0);
            // check how to format buttons 
            //sp_btnBemerkungen.Resources["Style"]

           
            
            dg.ItemsSource = funktions;
            dg.CellStyle = st;
            panelPrueba.Children.Add(dg);

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
