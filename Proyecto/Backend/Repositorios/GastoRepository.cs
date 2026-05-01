using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using System.Security.Cryptography.X509Certificates;

namespace Proyecto.Backend.Repositorios
{
    public class GastoRepository : GenericRepository<Gasto>, IGastoRepository
    {
        public GastoRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<Gasto>> logger) : base(contexto, logger)
        {
            

        }

        public async Task<List<Gasto>> GetGastosCompletosAsync()
        {
            return await _context.Set<Gasto>()
                // 1. Incluimos directamente la colección de Máquinas
                // NOTA: Verifica en tu archivo Gasto.cs si se llama "Maquinas" o "MaquinaIdMaquinas"
                .Include(g => g.MaquinaIdMaquinas)
                    // 2. Y luego cargamos los modelos de esas máquinas
                    .ThenInclude(m => m.IdModeloNavigation)
                .ToListAsync();
        }

    }
}
