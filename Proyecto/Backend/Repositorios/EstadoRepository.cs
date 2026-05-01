using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Backend.Repositorios
{
    public class EstadoRepository : GenericRepository<Estado>, IEstadoRepository
    {
        public EstadoRepository(AndresProyecto2Context context, ILogger<GenericRepository<Estado>> logger) : base(context, logger)
        {
        }
    }
}
