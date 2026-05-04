using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.Core;
using System.Windows;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {

        //private AndresProyecto2Context _context;

        private TrabajadorRepository _trabajadorRepository;
        private readonly MainWindow _mainWindow;

        public Login(TrabajadorRepository trabajadorRepository, MainWindow mainWindow)
        {
            InitializeComponent();
            _trabajadorRepository = trabajadorRepository;
            _mainWindow = mainWindow;
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(passClave.Password))
            {
                var  usuarioAutenticado = await _trabajadorRepository.LoginAsync2(txtUsuario.Text, passClave.Password);
                if (usuarioAutenticado != null)
                {
                    SesionGlobal.UsuarioActual = usuarioAutenticado;

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
    }
}