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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar La Máquina", ex.Message);
            }
        }
    }
}
