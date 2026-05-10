using Proyecto.Backend.Modelo;
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
using System.Windows.Shapes;

namespace Proyecto.Dialogos
{
    /// <summary>
    /// Lógica de interacción para AgregarUsuario.xaml
    /// </summary>
    public partial class AgregarUsuario : Window
    {
        private MVTrabajador _MVAgregarUsuario;
        private readonly IServiceProvider _serviceProvider;
        public AgregarUsuario(MVTrabajador mVAgregarUsuario, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _MVAgregarUsuario = mVAgregarUsuario;
            _serviceProvider = serviceProvider;
        }

        public async Task Inicializa(Trabajadore trabajador)
        {
            await _MVAgregarUsuario.Inicializa();
            _MVAgregarUsuario.trabajador = trabajador; 
            DataContext = _MVAgregarUsuario;
        }

        private async void diagAgregarUsuario_Loaded(object sender, RoutedEventArgs e)
        {
            await _MVAgregarUsuario.Inicializa();
            DataContext = _MVAgregarUsuario;
        }

        private void btnCancelarUsuario_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void btnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _MVAgregarUsuario.GuardarUsuario();
                DialogResult = true;
                MessageBox.Show("Usuario guardado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar el Usuario");
            }
        }

        #region Botones ventana

        private void btnMin_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMax_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void btnClose_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
