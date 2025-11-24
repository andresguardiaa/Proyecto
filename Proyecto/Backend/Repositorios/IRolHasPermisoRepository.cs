using di.proyecto.clase._2025.Backend.Servicios;
using Proyecto.Backend.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Backend.Repositorios
{
    internal interface IRolHasPermisoRepository : IGenericRepository<RolHasPermiso>
    {

        Task<bool> LoginAsync(string username, string password);

        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);

        Task<RolHasPermiso?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

        Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);

    }
}
