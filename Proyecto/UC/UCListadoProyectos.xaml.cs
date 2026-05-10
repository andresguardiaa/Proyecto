using Microsoft.Extensions.DependencyInjection;
using Proyecto.Dialogos;
using Proyecto.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto.UC
{
    /// <summary>
    /// Lógica de interacción para UCListadoProyectos.xaml
    /// </summary>
    public partial class UCListadoProyectos : UserControl
    {
        private MVProyecto _mVProyecto;
        private AgregarProyecto _agregarProyecto;
        private readonly IServiceProvider _serviceProvider;
        public UCListadoProyectos(MVProyecto mVProyecto, AgregarProyecto agregarProyecto, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _mVProyecto = mVProyecto;
            _agregarProyecto = agregarProyecto;
            _serviceProvider = serviceProvider;
        }

        private async void listadoProyectos_Loaded(object sender, RoutedEventArgs e)
        {
            await _mVProyecto.Inicializa();
            this.DataContext = _mVProyecto;
        }

        private async void btnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            if (boton != null) boton.IsEnabled = false;

            await _mVProyecto.Inicializa();

            if (boton != null) boton.IsEnabled = true;
        }

        private async void btnEditarProyecto_Click(object sender, RoutedEventArgs e)
        {
            _agregarProyecto = _serviceProvider.GetRequiredService<AgregarProyecto>();
            await _agregarProyecto.Inicializa(_mVProyecto.ProyectoSeleccionado);
            _agregarProyecto.ShowDialog();
            if (_agregarProyecto.DialogResult == true)
            {
                await _mVProyecto.Inicializa();
            }
        }

        private async void btnEliminarProyecto_Click(object sender, RoutedEventArgs e)
        {
            if (_mVProyecto.ProyectoSeleccionado != null)
            {
                MessageBoxResult resultado = MessageBox.Show(
                    $"¿Estás seguro de que deseas dar de baja el proyecto '{_mVProyecto.ProyectoSeleccionado.Nombre}'?",
                    "Confirmar Baja",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (resultado == MessageBoxResult.Yes)
                {
                    await _mVProyecto.DarDeBajaProyecto(_mVProyecto.ProyectoSeleccionado);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un proyecto de la lista para dar de baja.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
