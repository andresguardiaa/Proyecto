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
    /// Lógica de interacción para UCDashboard.xaml
    /// </summary>
    public partial class UCDashboard : UserControl
    {
        private MVDashboard _mVDashboard;
        public UCDashboard(MVDashboard mVDashboard)
        {
            InitializeComponent();
            _mVDashboard = mVDashboard;
        }

        private async void ucDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            _mVDashboard.RefrescarSesion();
            this.DataContext = _mVDashboard;
            await _mVDashboard.InicializarDashboardAsync();
        }

        private async void btnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            if (boton != null) boton.IsEnabled = false;

            await _mVDashboard.InicializarDashboardAsync();

            if (boton != null) boton.IsEnabled = true;
        }
    }
}
