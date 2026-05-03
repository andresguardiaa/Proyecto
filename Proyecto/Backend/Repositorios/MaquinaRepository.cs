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
    public class MaquinaRepository : GenericRepository<Maquina>, IMaquinaRepository
    {
        public MaquinaRepository(AndresProyecto2Context context, ILogger<GenericRepository<Maquina>> logger) : base(context, logger)
        {

        }

      
        public async Task<IEnumerable<Maquina>> GetAllMaquinasAsync()
        {
            return await _context.Maquinas
                             .Include(m => m.IdEstadoNavigation) 
                             .ToListAsync();
        }
    }
    
}
