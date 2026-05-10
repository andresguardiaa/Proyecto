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
    /// Lógica de interacción para AgregarEstadoMaquina.xaml
    /// </summary>
    public partial class AgregarEstadoMaquina : Window
    {
        private MVEstado _mVEstado;
        public AgregarEstadoMaquina(MVEstado mVEstado)
        {
            InitializeComponent();
            _mVEstado = mVEstado;

            this.DataContext = _mVEstado;
        }

        private void btnCancelarEstado_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private async void btnGuardarEstado_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _mVEstado.GuardarEstado();
                DialogResult = true;
                MessageBox.Show("Estado guardado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar el Estado", ex.Message);
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
