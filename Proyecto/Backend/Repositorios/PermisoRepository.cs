using di.proyecto.clase._2025.Backend.Servicios;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;

namespace Proyecto.Backend.Repositorios
{
    public class PermisoRepository : GenericRepository<Permiso>, IPermisoRepository
    {
        public PermisoRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<Permiso>> logger) : base(contexto, logger)
        {
        }
    }
}
