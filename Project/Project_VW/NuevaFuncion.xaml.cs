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
            string nombreFunc = "";
            string jahr = "";
            string nar = "";
            string rdw = "";
            string gesetz = "";
            string kw = "";
            string descripcion = "";

            nombreFunc = NombreFuncion.Text;
            jahr = Jahr.Text;
            nar = NAR.Text;
            rdw = RdW.Text;
            gesetz = Gesetz.Text;
            kw = KW.Text;
            descripcion = DescripFuncion.Text;
            int ID_selectedSystem = Convert.ToInt32(chooseSystem.SelectedValue);
            db.openConn();

            string qry_addFunc = "INSERT INTO funktion (nombre, NAR, RDW, Gesetz, ";
            qry_addFunc += "KW, Jahr, descripcion, sistema_ID, editado_por) VALUES (";
            qry_addFunc += "'" + nombreFunc + "', ";
            qry_addFunc += "'" + nar + "', ";
            qry_addFunc += "'" + rdw + "', ";
            qry_addFunc += "'" + gesetz + "', ";
            qry_addFunc += "'" + kw + "', ";
            qry_addFunc += "'" + jahr + "', ";
            qry_addFunc += "'" + descripcion + "', ";
            qry_addFunc += ID_selectedSystem;
            qry_addFunc += ", '" + SesionUsuario.getUser() + "')";

            using (db.setComm(qry_addFunc))
            {
                affectedRows = db.getComm().ExecuteNonQuery();
            }
            if (affectedRows == 0)
            {
                db.sendMBandCloseConn("No se pudo crear las relaciones pertinentes de sistemas y autos.");
                return;
            }

            db.sendMBandCloseConn("Funcion creada exitosamente dentro del sistema: ." + chooseSystem.SelectedItem.ToString());
        }
    }
}
