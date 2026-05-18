using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Proyecto.Dialogos;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto.UC
{
    /// <summary>
    /// Lógica de interacción para UCConfiguracion.xaml
    /// </summary>
    public partial class UCConfiguracion : UserControl
    {
        private AgregarModelo _agregarModelo;
        private AgregarEstadoMaquina _agregarEstadoMaquina;
        private readonly IServiceProvider _serviceProvider;
        public UCConfiguracion(AgregarModelo agregarModelo, IServiceProvider serviceProvider, AgregarEstadoMaquina agregarEstadoMaquina)
        {
            InitializeComponent();
            _agregarModelo = agregarModelo;
            _serviceProvider = serviceProvider;
            _agregarEstadoMaquina = agregarEstadoMaquina;
        }

        private void btnGestionarModelos_Click(object sender, RoutedEventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<AgregarModelo>();
            dlg.ShowDialog();
        }

        private void btnGestionarEstados_Click(object sender, RoutedEventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<AgregarEstadoMaquina>();
            dlg.ShowDialog();
        }

        #region Copia de Seguridad
        private async void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Archivo de Base de Datos SQL (*.sql)|*.sql",
                Title = "Guardar Copia de Seguridad",
                FileName = $"AndresProyecto2_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql"
            };

            if (dialog.ShowDialog() == true)
            {
                string rutaArchivo = dialog.FileName;
                btnBackup.IsEnabled = false;

                try
                {
                    await Task.Run(() => RealizarVolcadoMySQL(rutaArchivo));
                    MessageBox.Show("El volcado de la base de datos se ha completado y guardado con éxito.", "Copia de Seguridad Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al intentar generar la copia de seguridad:\n\n{ex.Message}", "Error de Volcado", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    btnBackup.IsEnabled = true;
                }
            }
        }

        private void RealizarVolcadoMySQL(string rutaDestino)
        {
            string connectionString = "server=127.0.0.1;port=3306;database=andres_proyecto2;user=root;password=root";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        mb.ExportToFile(rutaDestino);

                        conn.Close();
                    }
                }
            }
        }

        #endregion
    }
}
