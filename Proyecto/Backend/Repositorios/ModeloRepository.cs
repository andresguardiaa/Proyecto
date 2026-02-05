using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using ClaseModelo = Proyecto.Backend.Modelo.Modelo;

namespace Proyecto.Backend.Repositorios
{
    public class ModeloRepository : GenericRepository<ClaseModelo>, IModeloRepository
    {

        public ModeloRepository(AndresProyecto2Context contexto, ILogger<GenericRepository<ClaseModelo>> logger) : base(contexto, logger)
        {
        }

    }
}
