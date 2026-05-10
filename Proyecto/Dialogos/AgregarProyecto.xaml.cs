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
    /// Lógica de interacción para AgregarProyecto.xaml
    /// </summary>
    public partial class AgregarProyecto : Window
    {

        private MVProyecto _MVProyecto;
        public AgregarProyecto(MVProyecto mVProyecto)
        {
            InitializeComponent();
            _MVProyecto = mVProyecto;
        }

        private async void diagAgregarProyecto_Loaded(object sender, RoutedEventArgs e)
        {
            await _MVProyecto.Inicializa();
            DataContext = _MVProyecto;
        }

        public async Task Inicializa(Backend.Modelo.Proyecto proyecto)
        {
            await _MVProyecto.Inicializa();
            _MVProyecto.proyecto = proyecto;
            DataContext = _MVProyecto;
        }

        private void btnCancelarProyecto_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void btnGuardarProyecto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _MVProyecto.GuardarProyecto();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar el proyecto");
            }
        }
    }
}
