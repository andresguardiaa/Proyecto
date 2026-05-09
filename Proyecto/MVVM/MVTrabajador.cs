using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.MVVM.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Proyecto.MVVM
{
    public class MVTrabajador : MVBase
    {

        ///Hereda de mvbase, para crear la lista y gestionar la validación

        

        private Trabajadore _trabajador;
        private TrabajadorRepository _trabajadorRepository;
        private RolRepository _rolRepository;

        
        private List<Rol> _listaRoles;
        private ObservableCollection<Trabajadore> _listaTrabajadores;
        private Trabajadore _trabajadorSeleccionado;

        public List<Rol> listaRoles => _listaRoles;
        public ObservableCollection<Trabajadore> listaTrabajadores
        {
            get => _listaTrabajadores;
            set => SetProperty(ref _listaTrabajadores, value);
        }

        public Trabajadore trabajador
        {
            get => _trabajador;
            set => SetProperty(ref _trabajador, value);
        }

        public Trabajadore trabajadorSeleccionado
        { 
            get => _trabajadorSeleccionado;
            set => SetProperty(ref _trabajadorSeleccionado, value);
        }
        

        public MVTrabajador(TrabajadorRepository trabajadorRepository, RolRepository rolRepository)
        {
            _trabajadorRepository = trabajadorRepository;
            _rolRepository = rolRepository;
            trabajador = new Trabajadore();
        }

        public async Task Inicializa()
        {
            _listaRoles = await GetAllAsync<Rol>(_rolRepository);
            var trabajadores = await GetAllAsync<Trabajadore>(_trabajadorRepository);
            listaTrabajadores = new ObservableCollection<Trabajadore>(trabajadores);
        }

        public async Task GuardarUsuario()
        {

            var usuarioExistente = await _trabajadorRepository.FirstOrDefaultAsync(x => x.Usuario == trabajador.Usuario);
            try
            {
                if (usuarioExistente == null)
                {
                    await _trabajadorRepository.AddAsync(trabajador);
                }
                else
                {
                    await _trabajadorRepository.UpdateAsync(trabajador);
                }
                SnackbarMessageQueue.Enqueue("Usuario guardado correctamente.");
            }
            catch (Exception ex)
            {
                SnackbarMessageQueue.Enqueue($"Error al guardar el usuario: {ex.Message}");
            }
        }

        public async Task BorrarUsuario(Trabajadore trabajadorABorrar)
        {
            if (trabajadorABorrar == null) return;

            try
            {
                trabajadorABorrar.Estado = "Baja"; 

                await _trabajadorRepository.UpdateAsync(trabajadorABorrar);

                await Inicializa();
                SnackbarMessageQueue.Enqueue("Usuario dado de baja correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el estado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
