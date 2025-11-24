using Castle.Core.Logging;
using di.proyecto.clase._2025.Backend.Servicios;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using MahApps.Metro.Controls;
using Proyecto.Backend.Repositorios;
using System.Windows;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {

        private AndresProyecto2Context _context;
        //private GenericRepository<RolHasPermiso> _rolHasPermisoRepository;
        private RolHasPermisoRepository _rolHasPermisoRepository;
        private ILogger<GenericRepository<RolHasPermiso>> _logger;  

        public Login()
        {
            InitializeComponent();
            _context = new AndresProyecto2Context();
            _rolHasPermisoRepository = new RolHasPermisoRepository(_context, new LoggerFactory().CreateLogger<GenericRepository<RolHasPermiso>>());
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(passClave.Password))
            {
                bool isAuthenticated = await _rolHasPermisoRepository.LoginAsync(txtUsuario.Text, passClave.Password);
                if (isAuthenticated)
                {
                    MainWindow ventanaPrincipal = new MainWindow();
                    ventanaPrincipal.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Usuario o clave incorrectos.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);

                }
               

            }
            else
            {
                MessageBox.Show("Por favor, introduce usuario y clave.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {

            _context = new AndresProyecto2Context();
            _logger = new LoggerFactory().CreateLogger<GenericRepository<RolHasPermiso>>();
            _rolHasPermisoRepository = new RolHasPermisoRepository(_context, _logger);

        }

        private void btnMin_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            this.WindowState = WindowState.Minimized;

        }

        private void btnMax_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if(this.WindowState == WindowState.Normal)
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
    }
}