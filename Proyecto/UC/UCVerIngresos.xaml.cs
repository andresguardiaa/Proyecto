using Proyecto.MVVM;
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

namespace Proyecto.UC
{
    /// <summary>
    /// Interaction logic for UCVerIngresos.xaml
    /// </summary>
    public partial class UCVerIngresos : UserControl
    {
        private MVFactura _mVFactura;
        public UCVerIngresos(MVFactura mVFactura)
        {
            InitializeComponent();
            _mVFactura = mVFactura;
        }

        private async void ListadoIngresos_Loaded(object sender, RoutedEventArgs e)
        {
            await _mVFactura.Inicializa();
            this.DataContext = _mVFactura;
        }
    }
}
