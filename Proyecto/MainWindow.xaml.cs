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
        private AgregarProyecto _agregarProyecto;
        private AgregarMaquina _agregarMaquina;
        private UCVerGastos _uCVerGastos;
        private UCVerIngresos _uCVerIngresos;
        private UCDashboard _uCDashboard;

        //Constructor
        public MainWindow(
            AgregarUsuario agregarUsuario, 
            AgregarProyecto agregarProyecto, 
            AgregarMaquina agregarMaquina, 
            UCVerGastos uCVerGastos, 
            UCVerIngresos uCVerIngresos,
            UCDashboard uCDashboard
            )
        {
            InitializeComponent();
            _agregarUsuario = agregarUsuario;
            _agregarProyecto = agregarProyecto;
            _agregarMaquina = agregarMaquina;
            _uCVerGastos = uCVerGastos;
            _uCVerIngresos = uCVerIngresos;
            _uCDashboard = uCDashboard;

            CargarDashboard();
        }

        #region dialogos 

        private void AgregarUsuario_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = _agregarUsuario;
            dlg.ShowDialog();

        }

        private void AgregarProyecto_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = _agregarProyecto;
            dlg.ShowDialog();
        }

        private void AgregarMaquina_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = _agregarMaquina;
            dlg.ShowDialog();
        }

        #endregion

        #region botones de ventana
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
            Application.Current.Shutdown();
        }
        #endregion

        #region UserControl

        //Carga inicial del dashboard
        private void CargarDashboard()
        {
            if (panelCentral != null) panelCentral.Children.Clear();
            panelCentral.Children.Add(_uCDashboard);
        }

        private void btnDashboard_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CargarDashboard();
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

        

        #endregion

        
    }
}
