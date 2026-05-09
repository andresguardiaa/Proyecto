using Microsoft.Extensions.DependencyInjection;
using Proyecto.Dialogos;
using Proyecto.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto.UC
{
    /// <summary>
    /// Lógica de interacción para UCListadoUsuarios.xaml
    /// </summary>
    public partial class UCListadoUsuarios : UserControl
    {
        private MVTrabajador _mVTrabajador;
        private AgregarUsuario _agregarUsuario;
        private readonly IServiceProvider _serviceProvider;
        public UCListadoUsuarios(MVTrabajador mVTrabajador, AgregarUsuario agregarUsuario, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _mVTrabajador = mVTrabajador;
            _agregarUsuario = agregarUsuario;
            _serviceProvider = serviceProvider;
        }

        private async void ucListadoUsuarios_Loaded(object sender, RoutedEventArgs e)
        {
            await _mVTrabajador.Inicializa();
            this.DataContext = _mVTrabajador;
        }

        private async void btnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            _agregarUsuario = _serviceProvider.GetRequiredService<AgregarUsuario>();
            await _agregarUsuario.Inicializa(_mVTrabajador.trabajadorSeleccionado);
            _agregarUsuario.ShowDialog();
            if(_agregarUsuario.DialogResult == true) 
            {
                
            }
        }

        private async void btnEliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (_mVTrabajador.trabajadorSeleccionado != null)
            {
                MessageBoxResult confirmacion = MessageBox.Show(
                    $"¿Estás seguro de que deseas dar de baja a {_mVTrabajador.trabajadorSeleccionado.Nombre} {_mVTrabajador.trabajadorSeleccionado.Apellido1}?",
                    "Confirmar Eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirmacion == MessageBoxResult.Yes)
                {
                    await _mVTrabajador.BorrarUsuario(_mVTrabajador.trabajadorSeleccionado);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario de la lista antes de eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void btnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            if (boton != null) boton.IsEnabled = false;

            await _mVTrabajador.Inicializa();

            if (boton != null) boton.IsEnabled = true;
        }

    }
}
