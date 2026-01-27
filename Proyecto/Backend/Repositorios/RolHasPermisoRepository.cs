using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;

namespace Proyecto.Backend.Repositorios
{
    public class RolHasPermisoRepository : GenericRepository<RolHasPermiso>, IRolHasPermisoRepository
    {
     
        public RolHasPermisoRepository(AndresProyecto2Context context, ILogger<GenericRepository<RolHasPermiso>> logger)
            : base(context, logger)
        {
            
        }

        
        
    }
}
