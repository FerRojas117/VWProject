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
    /// Lógica de interacción para CambiarEvento.xaml
    /// </summary>
    public partial class CambiarEvento : UserControl
    {
        List<ComboBoxPairsEvento> cbp;
        DB db;

        public CambiarEvento()
        {
            db = new DB();
            InitializeComponent();
            fillEventos();
        }

        public void fillEventos()
        {
            cbp = new List<ComboBoxPairsEvento>();
            string qry_getEventos = "SELECT ID, nombre FROM evento WHERE is_current = 2";
            db.openConn();
            using (db.setComm(qry_getEventos))
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
            comboEventos.DisplayMemberPath = "nombre";
            comboEventos.SelectedValuePath = "ID";
            comboEventos.ItemsSource = cbp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxPairsEvento cbp = (ComboBoxPairsEvento)comboEventos.SelectedItem;
            string evento_selected = cbp.nombre;
            int ID_evento = Convert.ToInt32(cbp.ID);
            MessageBoxResult result = MessageBox.Show(
                "Estás seguro de cambiar de evento: " + evento_selected + ", con ID: " + ID_evento + "?",
                "Eliminar Usuario",
                MessageBoxButton.YesNoCancel
            );

            switch (result)
            {
                case MessageBoxResult.Yes:
                    MessageBox.Show("ES: " + evento_selected + ", IDEV: " + ID_evento);
                    SesionUsuario.setEvento(evento_selected);
                    SesionUsuario.setIDEvento(ID_evento);
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Usuario No eliminado.", "Eliminar Usuario");
                    db.closeConn();
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Usuario No eliminado.", "Eliminar Usuario");
                    db.closeConn();
                    break;
            }
        }
    }
    public class ComboBoxPairsEvento
    {
        public string nombre { get; set; }
        public string ID { get; set; }

        public ComboBoxPairsEvento(string user_p, string ID_P)
        {
            nombre = user_p;
            ID = ID_P;
        }
    }
}



 
