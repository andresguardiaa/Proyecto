using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Backend.Repositorios
{
    public class ProyectoRepository : GenericRepository<Modelo.Proyecto>, IProyectoRepository
    {
        public ProyectoRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<Modelo.Proyecto>> logger) : base(contexto, logger)
        {

        }

    }
}
