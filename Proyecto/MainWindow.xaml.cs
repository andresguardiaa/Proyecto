using Proyecto.Dialogos;
using Proyecto.UC;
using System.Windows;

namespace Proyecto
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AgregarUsuario _agregarUsuario;
        private UCVerGastos _uCVerGastos;
        private UCVerIngresos _uCVerIngresos;
        public MainWindow(AgregarUsuario agregarUsuario, UCVerGastos uCVerGastos, UCVerIngresos uCVerIngresos)
        {
            InitializeComponent();
            _agregarUsuario = agregarUsuario;
            _uCVerGastos = uCVerGastos;
            _uCVerIngresos = uCVerIngresos;
        }

        private void AgregarUsuario_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = _agregarUsuario;
            dlg.ShowDialog();

        }

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

        private void btnListarGastos_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(panelCentral != null) panelCentral.Children.Clear();
            panelCentral.Children.Add(_uCVerGastos);
        }

        private void btnListarIngresos_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (panelCentral != null) panelCentral.Children.Clear();
            panelCentral.Children.Add(_uCVerIngresos);
        }
    }
}
