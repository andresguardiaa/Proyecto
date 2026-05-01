using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.MVVM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.MVVM
{
    public class MVTrabajador : MVBase
    {

        ///Hereda de mvbase, para crear la lista y gestionar la validación

        

        private Trabajadore _trabajador;
        private TrabajadorRepository _trabajadorRepository;
        private RolRepository _rolRepository;

        
        private List<Rol> _listaRoles;

        public List<Rol> listaRoles => _listaRoles;
        

        public Trabajadore trabajador
        {
            get => _trabajador;
            set => SetProperty(ref _trabajador, value);
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
    }
}
