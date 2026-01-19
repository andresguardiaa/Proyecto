using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;

namespace Proyecto.Backend.Repositorios
{
    public class GastoRepository : GenericRepository<Gasto>, IGastoRepository
    {
        public GastoRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<Gasto>> logger) : base(contexto, logger)
        {
        }
    }
}
