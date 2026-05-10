using Microsoft.Extensions.DependencyInjection;
using Proyecto.Dialogos;
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
    /// Lógica de interacción para UCConfiguracion.xaml
    /// </summary>
    public partial class UCConfiguracion : UserControl
    {
        private AgregarModelo _agregarModelo;
        private AgregarEstadoMaquina _agregarEstadoMaquina;
        private readonly IServiceProvider _serviceProvider;
        public UCConfiguracion(AgregarModelo agregarModelo, IServiceProvider serviceProvider, AgregarEstadoMaquina agregarEstadoMaquina)
        {
            InitializeComponent();
            _agregarModelo = agregarModelo;
            _serviceProvider = serviceProvider;
            _agregarEstadoMaquina = agregarEstadoMaquina;
        }

        private void btnGestionarModelos_Click(object sender, RoutedEventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<AgregarModelo>();
            dlg.ShowDialog();
        }

        private void btnGestionarEstados_Click(object sender, RoutedEventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<AgregarEstadoMaquina>();
            dlg.ShowDialog();
        }
    }
}
