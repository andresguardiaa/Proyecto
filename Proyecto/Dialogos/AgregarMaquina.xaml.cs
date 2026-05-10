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
    /// Lógica de interacción para AgregarMaquina.xaml
    /// </summary>
    public partial class AgregarMaquina : Window
    {
        private MVMaquina _MVMaquina;
        public AgregarMaquina(MVMaquina mVMaquina)
        {
            InitializeComponent();
            _MVMaquina = mVMaquina;
        }

        public async Task Inicializa(Maquina maquina)
        {
            await _MVMaquina.Inicializa();
            _MVMaquina.maquina = maquina;
            if (maquina != null)
            {
                _MVMaquina.NombreModelo = maquina.IdModeloNavigation?.ModeloMaquina;
                _MVMaquina.NombreEstado = maquina.IdEstadoNavigation?.Descripcion;
            }
            DataContext = _MVMaquina;
        }

        private async void diagAgregarMaquina_Loaded(object sender, RoutedEventArgs e)
        {
            await _MVMaquina.Inicializa();
            DataContext = _MVMaquina;
        }

        private void btnCancelarMaquina_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void btnGuardarMaquina_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _MVMaquina.GuardarMaquina();
                DialogResult = true;
                MessageBox.Show("Máquina guardada correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar La Máquina", ex.Message);
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
