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
    /// Lógica de interacción para funcionesSistema.xaml
    /// </summary>
    public partial class funcionesSistema : UserControl
    {
        string evento, vehiculo, sistema;
        public funcionesSistema(string evento, string vehiculo, string sistema)
        {
            this.evento = evento;
            this.vehiculo = vehiculo;
            this.sistema = sistema;
            InitializeComponent();           
        }


    }
}
