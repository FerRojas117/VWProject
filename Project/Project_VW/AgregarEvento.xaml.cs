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
    /// Lógica de interacción para AgregarEvento.xaml
    /// </summary>
    public partial class AgregarEvento : UserControl
    {
        DB db;
        public AgregarEvento()
        {
            db = new DB();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string evento = evento_box.Text;
            string descripcion = descripcion_box.Text;
            int affectedRows = 0;
            string qry_getEvents = "SELECT COUNT(*) FROM evento";
            db.openConn();
            using (db.setComm(qry_getEvents))
            {
                db.setReader();
                while (db.getReader().Read())
                {
                    affectedRows = Convert.ToInt32(db.getReader()["numEventos"]);
                }
            }
            if (affectedRows == 0)
            {
                db.sendMBandCloseConn("No hay eventos registrados. Este se convertirá en el evento por defecto para todos.");    
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                   "Quieres hacer de este evento, el evento por defecto para todos?",
                   "CrearEvento",
                   MessageBoxButton.YesNoCancel
                );
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string qry_update = "UPDATE evento SET is_current = 2 WHERE is_current = 1;";
                        using (db.setComm(qry_update))
                        { }
                        string qry_newEvent = "INSERT INTO evento (nombre, descripcion, is_current)";
                        qry_newEvent += "VALUES('" + evento + "', '" + descripcion + "', 1)";
                        using (db.setComm(qry_newEvent))
                        {
                            affectedRows = db.getComm().ExecuteNonQuery();
                        }
                        if (affectedRows == 0)
                        {
                            db.sendMBandCloseConn("No se pudo crear evento. Inténtalo de nuevo");
                            return;
                        }
                        db.sendMBandCloseConn("Evento agregado exitosamente. Establecido como evento por defecto.");
                        break;
                    case MessageBoxResult.No:
                        string qry_newEvent_no = "INSERT INTO evento (nombre, descripcion, is_current)";
                        qry_newEvent_no += "VALUES('" + evento + "', '" + descripcion + "', 2)";
                        using (db.setComm(qry_newEvent_no))
                        {
                            affectedRows = db.getComm().ExecuteNonQuery();
                        }
                        if (affectedRows == 0)
                        {
                            db.sendMBandCloseConn("No se pudo crear evento. Inténtalo de nuevo");
                            return;
                        }
                        db.sendMBandCloseConn("Evento agregado exitosamente. No fué establecido como evento por defecto.");
                        break;
                    case MessageBoxResult.Cancel:
                        MessageBox.Show("Creación de evento cancelada", "Crear Evento");
                        break;
                }
            }
        }
    }
}
