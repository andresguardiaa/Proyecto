using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.MVVM.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace Proyecto.MVVM
{
    public class MVProyecto : MVBase
    {

        private Backend.Modelo.Proyecto _proyecto;
        private ProyectoRepository _proyectoRepository;
        private ClienteRepository _clienteRepository;

        private List<Cliente> _listaClientes;
        private ObservableCollection<Backend.Modelo.Proyecto> _listaProyectos;
        private Backend.Modelo.Proyecto _proyectoSeleccionado;


        public List<Cliente> listaClientes => _listaClientes;

        public Backend.Modelo.Proyecto proyecto
        {
            get => _proyecto;
            set => SetProperty(ref _proyecto, value);
        }

        public ObservableCollection<Backend.Modelo.Proyecto> ListaProyectos
        {
            get => _listaProyectos;
            set => SetProperty(ref _listaProyectos, value);
        }

        public Backend.Modelo.Proyecto ProyectoSeleccionado
        {
            get => _proyectoSeleccionado;
            set => SetProperty(ref _proyectoSeleccionado, value);
        }

        public MVProyecto(ProyectoRepository proyectoRepository, ClienteRepository clienteRepository) 
        {
            _clienteRepository = clienteRepository;
            _proyectoRepository = proyectoRepository;
            proyecto = new Backend.Modelo.Proyecto();
        }

        public async Task Inicializa()
        {
            _listaClientes = await GetAllAsync<Cliente>(_clienteRepository);
            var proyectos = await GetAllAsync<Backend.Modelo.Proyecto>(_proyectoRepository);
            ListaProyectos = new ObservableCollection<Backend.Modelo.Proyecto>(proyectos);
        }

        public async Task GuardarProyecto()
        {
            var proyectoExistente = await _proyectoRepository.FirstOrDefaultAsync(x => x.Nombre == proyecto.Nombre);
            try
            {
                if (proyectoExistente == null)
                {
                    await _proyectoRepository.AddAsync(proyecto);
                }
                else
                {
                    await _proyectoRepository.UpdateAsync(proyecto);
                }
                SnackbarMessageQueue.Enqueue("Proyecto guardado correctamente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el proyecto: " + ex.Message);
            }
        }

        public async Task DarDeBajaProyecto(Backend.Modelo.Proyecto proyectoABorrar)
        {
            if (proyectoABorrar == null) return;

            try
            {
                proyectoABorrar.FechaFin = DateTime.Now;

                // proyectoABorrar.Estado = "Baja"; 

                await _proyectoRepository.UpdateAsync(proyectoABorrar);

                await Inicializa(); 
                SnackbarMessageQueue.Enqueue("Proyecto dado de baja correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al dar de baja el proyecto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
