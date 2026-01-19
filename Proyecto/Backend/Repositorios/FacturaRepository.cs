using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;

namespace Proyecto.Backend.Repositorios
{
    public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
    {
        public FacturaRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<Factura>> logger) : base(contexto, logger)
        {
        }

        public async Task<List<Factura>> GetFacturasConProyectosAsync()
        {
            // Asumo que tu DbContext se llama _context dentro del repositorio
            return await _context.Facturas
                                 .Include(f => f.ProyectosIdProyectoNavigation) // <--- ESTA ES LA CLAVE
                                 .ToListAsync();
        }
    }
}
