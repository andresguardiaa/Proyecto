using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Backend.Repositorios
{
    public class TrabajoRepository : GenericRepository<Trabajo>, ITrabajoRepository
    {
        public TrabajoRepository(AndresProyecto2Context context, ILogger<GenericRepository<Trabajo>> logger)
            : base(context, logger)
        {
        }
    }
}
