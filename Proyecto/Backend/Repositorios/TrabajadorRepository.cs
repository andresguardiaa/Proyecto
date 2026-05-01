using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Backend.Repositorios
{
    public class TrabajadorRepository : GenericRepository<Trabajadore>
    {
        private readonly ILogger<GenericRepository<Trabajadore>> _logger;
        public TrabajadorRepository(AndresProyecto2Context context, ILogger<GenericRepository<Trabajadore>> logger)
            : base(context, logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
        }


        /// <summary>
        /// Intenta autenticar por el campo Usuario y Password de la tabla Trabajadores.
        /// Devuelve true si las credenciales coinciden.
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                var trabajador = await Query(asNoTracking: true)
                    .FirstOrDefaultAsync(t => t.Usuario == username, cancellationToken)
                    .ConfigureAwait(false);

                return trabajador != null && trabajador.Password == password;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar Trabajador usuario {Username}.", username);
                throw;
            }
        }

        /// <summary>
        /// Cambia la contraseña identificando el registro por su campo Usuario.
        /// Devuelve true si se actualizó correctamente.
        /// </summary>
        public async Task<bool> ChangePasswordAsync(string username, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("La nueva contraseña no puede estar vacía.", nameof(newPassword));

            try
            {
                // Buscamos con tracking (asNoTracking: false) para poder actualizar
                var trabajador = await Query(asNoTracking: false)
                    .FirstOrDefaultAsync(t => t.Usuario == username, cancellationToken)
                    .ConfigureAwait(false);

                if (trabajador == null)
                {
                    _logger.LogWarning("Cambio de contraseña: usuario {Username} no encontrado.", username);
                    return false;
                }

                if (trabajador.Password != currentPassword)
                {
                    _logger.LogWarning("Cambio de contraseña fallido: contraseña actual incorrecta para usuario {Username}.", username);
                    return false;
                }

                trabajador.Password = newPassword;
                await UpdateAsync(trabajador, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Contraseña actualizada correctamente para usuario {Username}.", username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar la contraseña del usuario {Username}.", username);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un trabajador por su campo Usuario (sin tracking).
        /// </summary>
        public async Task<Trabajadore?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await Query(asNoTracking: true)
                         .FirstOrDefaultAsync(t => t.Usuario == username, cancellationToken)
                         .ConfigureAwait(false);
        }

        /// <summary>
        /// Comprueba si existe un trabajador con el campo Usuario proporcionado.
        /// </summary>
        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await Query(asNoTracking: true)
                         .AnyAsync(t => t.Usuario == username, cancellationToken)
                         .ConfigureAwait(false);
        }

        /// <summary>
        /// Obtiene el trabajador identificado por su ID e incluye su ROL.
        /// Devuelve entidad trackeada (asNoTracking: false) para permitir edición posterior.
        /// </summary>
        public async Task<Trabajadore?> GetWithRelationsAsync(int idTrabajador, CancellationToken cancellationToken = default)
        {
            // Adaptado: Ahora buscamos por idTrabajador e incluimos el Rol (RolIdRolNavigation)
            return await Query(asNoTracking: false,
                               t => t.RolIdRolNavigation) // Incluimos la navegación al Rol
                         .FirstOrDefaultAsync(t => t.IdTrabajador == idTrabajador, cancellationToken)
                         .ConfigureAwait(false);
        }

    }
}
