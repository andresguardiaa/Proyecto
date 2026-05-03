using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;

namespace Proyecto.Backend.Repositorios
{
    public class NominaRepository : GenericRepository<Nomina>, INominaRepository
    {    
        public NominaRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<Nomina>> logger) : base(contexto, logger)
        {
        }
    }
}
