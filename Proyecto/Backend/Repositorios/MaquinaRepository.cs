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
    }
}
