using Proyecto.Dialogos;
using System.Windows;

namespace Proyecto
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AgregarUsuario _agregarUsuario;
        public MainWindow(AgregarUsuario agregarUsuario)
        {
            InitializeComponent();
            _agregarUsuario = agregarUsuario;
        }

        private void AgregarUsuario_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = _agregarUsuario;
            dlg.ShowDialog();

        }
    }
}
