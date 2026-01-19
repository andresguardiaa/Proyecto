using Proyecto.Backend.Modelo;
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
        
        private RolHasPermisoRepository _rolHasPermisoRepository;
        private readonly MainWindow _mainWindow;

        public Login(RolHasPermisoRepository rolHasPermisoRepository, MainWindow mainWindow)
        {
            InitializeComponent();
            _rolHasPermisoRepository = rolHasPermisoRepository;
            _mainWindow = mainWindow;
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(passClave.Password))
            {
                bool isAuthenticated = await _rolHasPermisoRepository.LoginAsync(txtUsuario.Text, passClave.Password);
                if (isAuthenticated)
                {
                    _mainWindow.Show();
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