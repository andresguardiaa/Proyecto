using di.proyecto.clase._2025.Backend.Servicios;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Backend.Repositorios
{
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {

        public RolRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<Rol>> logger) : base(contexto, logger)
        {
        }
    }
}
