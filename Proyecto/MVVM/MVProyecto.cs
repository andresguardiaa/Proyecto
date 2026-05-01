using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.MVVM.Base;

namespace Proyecto.MVVM
{
    public class MVProyecto : MVBase
    {

        private Backend.Modelo.Proyecto _proyecto;
        private ProyectoRepository _proyectoRepository;
        private ClienteRepository _clienteRepository;

        private List<Cliente> _listaClientes;

        public List<Cliente> listaClientes => _listaClientes;

        public Backend.Modelo.Proyecto proyecto
        {
            get => _proyecto;
            set => SetProperty(ref _proyecto, value);
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

    }
}
