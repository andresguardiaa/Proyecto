using Microsoft.Extensions.DependencyInjection;
using Proyecto.Core;
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
        //variables privadas
        private AgregarUsuario _agregarUsuario;
        private AgregarProyecto _agregarProyecto;
        private AgregarMaquina _agregarMaquina;
        private UCVerGastos _uCVerGastos;
        private UCVerIngresos _uCVerIngresos;
        private UCDashboard _uCDashboard;
        private UCListadoUsuarios _uCListadoUsuarios;
        private UCListadoMaquinas _uCListadoMaquinas;
        private UCListadoProyectos _uCListadoProyectos;
        private UCConfiguracion _uCConfiguracion;

        private readonly IServiceProvider _serviceProvider;

        //Constructor
        public MainWindow(
            AgregarUsuario agregarUsuario, 
            AgregarProyecto agregarProyecto, 
            AgregarMaquina agregarMaquina, 
            UCVerGastos uCVerGastos, 
            UCVerIngresos uCVerIngresos,
            UCDashboard uCDashboard,
            UCListadoUsuarios uCListadoUsuarios,
            UCListadoMaquinas uCListadoMaquinas,
            UCListadoProyectos uCListadoProyectos,
            UCConfiguracion uCConfiguracion,
            IServiceProvider serviceProvider
            )
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _agregarUsuario = agregarUsuario;
            _agregarProyecto = agregarProyecto;
            _agregarMaquina = agregarMaquina;
            _uCVerGastos = uCVerGastos;
            _uCVerIngresos = uCVerIngresos;
            _uCDashboard = uCDashboard;
            _uCListadoUsuarios = uCListadoUsuarios;
            _uCListadoMaquinas = uCListadoMaquinas;
            _uCListadoProyectos = uCListadoProyectos;
            _uCConfiguracion = uCConfiguracion;

            CargarDashboard();
        }

        //muestra u oculta la opción de configuración según el rol del usuario
        public void ConfigurarPermisos()
        {
            if (SesionGlobal.UsuarioActual != null && SesionGlobal.UsuarioActual.RolIdRol == 1)
            {
                gridConfiguracion.Visibility = Visibility.Visible;
            }
        }

        #region dialogos 

        private void AgregarUsuario_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<AgregarUsuario>();
            dlg.ShowDialog();
        }

        private void AgregarProyecto_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<AgregarProyecto>();
            dlg.ShowDialog();
        }

        private void AgregarMaquina_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<AgregarMaquina>();
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

        private void ListadoUsuarios_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (panelCentral != null) panelCentral.Children.Clear();
            panelCentral.Children.Add(_uCListadoUsuarios);
        }

        private void ListadoMaquinas_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (panelCentral != null) panelCentral.Children.Clear();
            panelCentral.Children.Add(_uCListadoMaquinas);
        }

        private void ListadoProyectos_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (panelCentral != null) panelCentral.Children.Clear();
            panelCentral.Children.Add(_uCListadoProyectos);
        }

        private void btnConfiguracion_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (panelCentral != null) panelCentral.Children.Clear();
            panelCentral.Children.Add(_uCConfiguracion);
        }

        #endregion

    }
}
