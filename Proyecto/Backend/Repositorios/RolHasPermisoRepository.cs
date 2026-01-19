using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;

namespace Proyecto.Backend.Repositorios
{
    public class RolHasPermisoRepository : GenericRepository<RolHasPermiso>
    {
        private readonly ILogger<GenericRepository<RolHasPermiso>> _logger;

        /// <summary>
        /// Crea una nueva instancia de <see cref="RolHasPermisoRepository"/>.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="logger">Logger para el repositorio.</param>
        public RolHasPermisoRepository(AndresProyecto2Context context, ILogger<GenericRepository<RolHasPermiso>> logger)
            : base(context, logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Intenta autenticar por el campo Usuario y Password del registro RolHasPermiso.
        /// Devuelve true si las credenciales coinciden, false en caso contrario.
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                var entidad = await Query(asNoTracking: true)
                    .FirstOrDefaultAsync(r => r.Usuario == username, cancellationToken)
                    .ConfigureAwait(false);

                return entidad != null && entidad.Password == password;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar RolHasPermiso usuario {Username}.", username);
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
                var entidad = await Query(asNoTracking: false)
                    .FirstOrDefaultAsync(r => r.Usuario == username, cancellationToken)
                    .ConfigureAwait(false);

                if (entidad == null)
                {
                    _logger.LogWarning("Cambio de contraseña: usuario {Username} no encontrado.", username);
                    return false;
                }

                if (entidad.Password != currentPassword)
                {
                    _logger.LogWarning("Cambio de contraseña fallido: contraseña actual incorrecta para usuario {Username}.", username);
                    return false;
                }

                entidad.Password = newPassword;
                await UpdateAsync(entidad, cancellationToken).ConfigureAwait(false);

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
        /// Obtiene un registro por su campo Usuario (sin tracking).
        /// </summary>
        public async Task<RolHasPermiso?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await Query(asNoTracking: true)
                         .FirstOrDefaultAsync(r => r.Usuario == username, cancellationToken)
                         .ConfigureAwait(false);
        }

        /// <summary>
        /// Comprueba si existe un registro con el campo Usuario proporcionado.
        /// </summary>
        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await Query(asNoTracking: true)
                         .AnyAsync(r => r.Usuario == username, cancellationToken)
                         .ConfigureAwait(false);
        }

        /// <summary>
        /// Obtiene el registro identificado por la clave compuesta e incluye las entidades relacionadas.
        /// Devuelve entidad trackeada (asNoTracking: false) para permitir edición posterior.
        /// </summary>
        public async Task<RolHasPermiso?> GetWithRelationsAsync(int permisosId, int rolId, CancellationToken cancellationToken = default)
        {
            return await Query(asNoTracking: false,
                               r => r.PermisosIdPermisoNavigation,
                               r => r.RolIdRolNavigation)
                         .FirstOrDefaultAsync(r => r.PermisosIdPermiso == permisosId && r.RolIdRol == rolId, cancellationToken)
                         .ConfigureAwait(false);
        }
    }
}
