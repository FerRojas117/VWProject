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
    /// Lógica de interacción para NuevaFuncion.xaml
    /// </summary>
    public partial class NuevaFuncion : UserControl
    {
        List<CheckBoxPairsSistemas> cbp_browseSistema;
        DB db;
        int affectedRows = 0;
        public NuevaFuncion()
        {
            InitializeComponent();
            db = new DB();
            fillSistema();
        }

        public void fillSistema()
        {
            cbp_browseSistema = new List<CheckBoxPairsSistemas>();
            string qry_getEventos = "SELECT ID, nombre FROM sistema";
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
            chooseSystem.DisplayMemberPath = "nombre";
            chooseSystem.SelectedValuePath = "ID";
            chooseSystem.ItemsSource = cbp_browseSistema;
        }

        private void AddFunktion_Click(object sender, RoutedEventArgs e)
        {
            if (chooseSystem.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un sistema para agregar esta función a este.");
                return;
            }
                
            string nombreFunc = "";
            string nar = "";
            string rdw = "";
            string gesetz = "";
            string descripcion = "";

            nombreFunc = NombreFuncion.Text;
            nar = NAR.Text;
            rdw = RdW.Text;
            gesetz = Gesetz.Text;
            descripcion = DescripFuncion.Text;
            int ID_selectedSystem = Convert.ToInt32(chooseSystem.SelectedValue);
            db.openConn();

            string qry_addFunc = "INSERT INTO funktion (nombre, NAR, RDW, Gesetz, ";
            qry_addFunc += "descripcion, Einsatz_KWJahr, editado_por, sistema_ID, color) VALUES (";
            qry_addFunc += "'" + nombreFunc + "', ";
            qry_addFunc += "'" + nar + "', ";
            qry_addFunc += "'" + rdw + "', ";
            qry_addFunc += "'" + gesetz + "', ";
            qry_addFunc += "'" + descripcion + "', ";
            qry_addFunc += "'" + Einsatz_KWJahr + "', ";
            qry_addFunc += "'" + SesionUsuario.getUser() + "', ";
            qry_addFunc += ID_selectedSystem;
            qry_addFunc += ", " + "3" + ")";

            using (db.setComm(qry_addFunc))
            {
                affectedRows = db.getComm().ExecuteNonQuery();
            }
            if (affectedRows == 0)
            {
                db.sendMBandCloseConn("No se pudo crear la función.");
                return;
            }

            db.sendMBandCloseConn("Funcion creada exitosamente dentro del sistema: ." + chooseSystem.SelectedItem.ToString());
        }
    }
}
