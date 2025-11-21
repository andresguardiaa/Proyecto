using di.proyecto.clase._2025.Backend.Servicios;
using Proyecto.Backend.Modelo;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {

        private AndresProyecto2Context _context;
        private GenericRepository<RolHasPermiso> _rolHasPermisoRepository;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(passClave.Password))
            {
                /*bool isAuthenticated = await _usuarioRepository.LoginAsync(txtUsuario.Text, passClave.Password);
                if (isAuthenticated)
                {
                    MainWindow ventanaPrincipal = new MainWindow();
                    ventanaPrincipal.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Usuario o clave incorrectos.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);

                }*/
                MainWindow ventanaPrincipal = new MainWindow();
                ventanaPrincipal.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("Por favor, introduce usuario y clave.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}