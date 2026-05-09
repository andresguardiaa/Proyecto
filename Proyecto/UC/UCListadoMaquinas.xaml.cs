using Microsoft.Extensions.DependencyInjection;
using Proyecto.Dialogos;
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
    /// Lógica de interacción para UCListadoMaquinas.xaml
    /// </summary>
    public partial class UCListadoMaquinas : UserControl
    {
        private MVMaquina _mVMaquina;
        private AgregarMaquina _agregarMaquina;
        private readonly IServiceProvider _serviceProvider;
        public UCListadoMaquinas(MVMaquina mVMaquina, AgregarMaquina agregarMaquina, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _mVMaquina = mVMaquina;
            _agregarMaquina = agregarMaquina;
            _serviceProvider = serviceProvider;
        }

        private async void listadoMaquinas_Loaded(object sender, RoutedEventArgs e)
        {
            await _mVMaquina.Inicializa();
            this.DataContext = _mVMaquina;
        }

        private async void btnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            if (boton != null) boton.IsEnabled = false;

            await _mVMaquina.Inicializa();

            if (boton != null) boton.IsEnabled = true;
        }

        private async void btnEditarMaquina_Click(object sender, RoutedEventArgs e)
        {
            _agregarMaquina = _serviceProvider.GetRequiredService<AgregarMaquina>();
            await _agregarMaquina.Inicializa(_mVMaquina.MaquinaSeleccionada);
            _agregarMaquina.ShowDialog();
            if (_agregarMaquina.DialogResult == true)
            {
                await _mVMaquina.Inicializa();
            }
        }

        private async void btnEliminarMaquina_Click(object sender, RoutedEventArgs e)
        {
            if (_mVMaquina.MaquinaSeleccionada != null)
            {
                string nombreModelo = _mVMaquina.MaquinaSeleccionada.IdModeloNavigation?.ModeloMaquina ?? "Desconocido";

                MessageBoxResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de que deseas dar de baja la máquina modelo '{nombreModelo}' ubicada en '{_mVMaquina.MaquinaSeleccionada.Ubicacion}'?",
                    "Confirmar Baja",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirmacion == MessageBoxResult.Yes)
                {
                    await _mVMaquina.DarDeBajaMaquina(_mVMaquina.MaquinaSeleccionada);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una máquina de la lista antes de darla de baja.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
