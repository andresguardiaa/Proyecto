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
    /// Interaction logic for UCVerGastos.xaml
    /// </summary>
    public partial class UCVerGastos : UserControl
    {
        private MVGasto _mVGasto;
        public UCVerGastos(MVGasto mVGasto)
        {
            InitializeComponent();
            _mVGasto = mVGasto;
        }

        private async void ListadoGastos_Loaded(object sender, RoutedEventArgs e)
        {
            await _mVGasto.Inicializa();
            this.DataContext = _mVGasto;
        }


        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            _mVGasto.LimpiarFiltros();
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            _mVGasto.Filtrar();
        }
    }
}
