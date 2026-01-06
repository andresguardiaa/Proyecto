using Ejercicio7.MVVM.Base;
using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.MVVM
{
    public class MVAgregarUsuario : MVBase
    {

        ///Hereda de mvbase, para crear la lista y gestionar la validación

        /// <summary>
        /// Objeto modelo de articulo que estamos gestionando
        /// vinculado a la lista para mostrar y editar los datos
        /// </summary>
        /// 

        private RolHasPermiso _rolHasPermiso;
        private RolHasPermisoRepository _rolHasPermisoRepository;
        private RolRepository _rolRepository;
        private PermisoRepository _permisoRepository;

        private List<Permiso> _listaPermisos;
        private List<Rol> _listaRoles;

        public List<Rol> listaRoles => _listaRoles;
        public List<Permiso> listaPermisos => _listaPermisos;

        public RolHasPermiso rolHasPermiso
        {
            get => _rolHasPermiso;
            set => SetProperty(ref _rolHasPermiso, value);
        }

        public MVAgregarUsuario(RolHasPermisoRepository rolHasPermisoRepository, RolRepository rolRepository, PermisoRepository permisoRepository)
        {
            _rolHasPermisoRepository = rolHasPermisoRepository;
            _rolRepository = rolRepository;
            _permisoRepository = permisoRepository;
            rolHasPermiso = new RolHasPermiso();
        }

        public async Task Inicializa()
        {
            _listaRoles = await GetAllAsync<Rol>(_rolRepository);
            _listaPermisos = await GetAllAsync<Permiso>(_permisoRepository);
        }

        public async Task GuardarUsuario()
        {

            var usuarioExistente = await _rolHasPermisoRepository.FirstOrDefaultAsync(x => x.Usuario == rolHasPermiso.Usuario);
            try
            {
                if (usuarioExistente == null)
                {
                    await _rolHasPermisoRepository.AddAsync(rolHasPermiso);
                }
                else
                {
                    await _rolHasPermisoRepository.UpdateAsync(rolHasPermiso);
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
