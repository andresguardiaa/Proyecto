using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Backend.Repositorios
{
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<Cliente>> logger) : base(contexto, logger)
        {
        }
    }
}
