using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using System.Linq.Expressions;

namespace Proyecto.Backend.Repositorios
{
    /// <summary>
    /// Repositorio genérico que implementa <see cref="IGenericRepository{T}"/> para EF Core.
    /// Proporciona operaciones CRUD comunes y manejo centralizado de logging y errores.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Instancia del <see cref="AndresProyecto2Context"/> usada por el repositorio.
        /// </summary>
        protected readonly AndresProyecto2Context _context;

        /// <summary>
        /// <see cref="DbSet{T}"/> para el tipo de entidad.
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        private readonly ILogger<GenericRepository<T>> _logger;

        /// <summary>
        /// Crea una nueva instancia del repositorio.
        /// </summary>
        /// <param name="context">Contexto de base de datos (no nulo).</param>
        /// <param name="logger">Instancia de logger (no nulo).</param>
        /// <exception cref="ArgumentNullException">Si <paramref name="context"/> o <paramref name="logger"/> son nulos.</exception>
        public GenericRepository(AndresProyecto2Context context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        /// <summary>
        /// Construye una consulta base <see cref="IQueryable{T}"/> sobre el conjunto de entidades.
        /// Úsalo para componer consultas, opcionalmente sin tracking e incluyendo propiedades de navegación.
        /// </summary>
        /// <param name="asNoTracking">Si es true, la consulta devuelve <c>AsNoTracking()</c>.</param>
        /// <param name="includes">Propiedades de navegación a incluir.</param>
        /// <returns><see cref="IQueryable{T}"/> componible.</returns>
        public IQueryable<T> Query(bool asNoTracking = true, 
                                   params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking) query = query.AsNoTracking();
            if (includes != null)
            {
                foreach (var inc in includes) query = query.Include(inc);
            }
            return query;
        }

        /// <summary>
        /// Busca una entidad por clave primaria de forma asíncrona usando <c>FindAsync</c>.
        /// </summary>
        /// <param name="id">Valor de la clave primaria.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>La entidad encontrada o null.</returns>
        public async Task<T?> GetByIdAsync(object id)
        {
            var found = await _dbSet.FindAsync( id ).ConfigureAwait(false);
            return found;
        }

        /// <summary>
        /// Devuelve la primera entidad que cumple el predicado, o null si no hay coincidencias.
        /// Soporta includes y comportamiento opcional sin tracking.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <param name="asNoTracking">Si es true, la consulta se ejecuta sin tracking.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <param name="includes">Propiedades de navegación a incluir.</param>
        /// <returns>Primera entidad que cumple la condición o null.</returns>
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, 
            bool asNoTracking = true, CancellationToken cancellationToken = default, 
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = Query(asNoTracking, includes);
            return await query.FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Este método busca la primera entidad que cumple con el filtro proporcionado.
        /// </summary>
        /// <param name="filter">Expresión booleana que filtra la lista</param>
        /// <returns>Devuelve la entidad o nulo en caso de no encontrar nada</returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter)
        {   
            T? entity = null;
            try
            {
                // Definimos una query
                IQueryable<T> query = _dbSet;

                // Aplicar filtro
                if (filter != null)
                {
                    // Añadimos el filtro a la query
                    query = query.Where(filter);
                }
                // Ejecutamos la query y obtenemos la primera entidad o nulo
                entity = await query.AsNoTracking().FirstOrDefaultAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la primera entidad de tipo {EntityName}",entity);
                throw new DataAccessException($"Error al obtener la primera entidad de tipo {entity}", ex);
            }
        }

        /// <summary>
        /// Recupera todas las entidades del tipo <typeparamref name="T"/> usando el <see cref="DbSet{T}"/>.
        /// Devuelve entidades trackeadas adjuntas al ChangeTracker del contexto.
        /// </summary>
        /// <returns>Todas las entidades como lista.</returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Busca entidades que coinciden con el predicado proporcionado.
        /// Usa <see cref="Query"/> si necesitas comportamiento distinto de tracking o includes.
        /// </summary>
        /// <param name="predicate">Expresión de filtrado.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <param name="asNoTracking">Si es true, la consulta se ejecuta sin tracking.</param>
        /// <param name="includes">Propiedades de navegación a incluir.</param>
        /// <returns>Lista con las entidades que coinciden.</returns>
        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
        {
            return await Query(asNoTracking, includes).Where(predicate).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Añade una entidad y persiste los cambios en la base de datos.
        /// Las excepciones se registran y se relanzan.
        /// </summary>
        /// <param name="entity">Entidad a añadir.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                await _dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Entidad de tipo {EntityType} añadida correctamente.", typeof(T).FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al añadir la entidad de tipo {EntityType}.", typeof(T).FullName);
                throw new DataAccessException($"Error al añadir la entidad de tipo {entity}", ex);
            }
        }

        /// <summary>
        /// Añade múltiples entidades y persiste los cambios en la base de datos.
        /// Las excepciones se registran y se relanzan.
        /// </summary>
        /// <param name="entities">Entidades a añadir.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                await _dbSet.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                int count = entities is ICollection<T> col ? col.Count : entities.Count();
                _logger.LogInformation("Se añadieron correctamente {Count} entidades del tipo {EntityType}.", count, typeof(T).FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al añadir un conjunto de entidades del tipo {EntityType}.", typeof(T).FullName);
                throw new DataAccessException($"Error al añadir un conjunto de entidades del tipo {typeof(T).FullName}", ex);
            }
        }

        /// <summary>
        /// Marca una entidad como modificada y persiste los cambios en la base de datos.
        /// Las excepciones se registran y se relanzan.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Entidad de tipo {EntityType} actualizada correctamente.", typeof(T).FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la entidad de tipo {EntityType}.", typeof(T).FullName);
                throw new DataAccessException($"Error al al actualizar la entidad del tipo {typeof(T).FullName}", ex);
            }
        }

        /// <summary>
        /// Elimina una entidad y persiste los cambios en la base de datos.
        /// Las excepciones se registran y se relanzan.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Entidad de tipo {EntityType} eliminada correctamente.", typeof(T).FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la entidad de tipo {EntityType}.", typeof(T).FullName);
                throw new DataAccessException($"Error al al eliminar la entidad del tipo {typeof(T).FullName}", ex);
            }
        }

        /// <summary>
        /// Elimina una entidad por clave primaria si existe y persiste los cambios.
        /// Registra una advertencia cuando la entidad no se encuentra.
        /// </summary>
        /// <param name="id">Valor de la clave primaria.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        public async Task RemoveByIdAsync(object id)
        {
            try
            {
                var entity = await GetByIdAsync(id).ConfigureAwait(false);
                if (entity == null)
                {
                    _logger.LogWarning("Entidad de tipo {EntityType} con id {Id} no encontrada para eliminación.", typeof(T).FullName, id);
                    return;
                }

                await RemoveAsync(entity).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la entidad con id {Id} del tipo {EntityType}.", id, typeof(T).FullName);
                throw new DataAccessException($"Error al eliminar la entidad del tipo {typeof(T).FullName}", ex);
            }
        }

        
        /// <summary>
        /// Guarda todos los cambios pendientes en el contexto a la base de datos de forma asíncrona.
        /// </summary>
        /// <returns>Número de registros afectados en la base de datos.</returns>
        /// <exception cref="DataAccessException">Se lanza cuando ocurre un error al guardar los cambios.</exception>
        /// <remarks>
        /// Este método persiste todas las operaciones de agregar, actualizar y eliminar que se hayan 
        /// realizado desde el último SaveChangesAsync o desde la creación del contexto.
        /// 
        /// Maneja específicamente:
        /// - DbUpdateConcurrencyException: cuando ocurren conflictos de concurrencia
        /// - DbUpdateException: cuando hay errores de actualización de base de datos
        /// - Exception: cualquier otro error inesperado
        /// 
        /// En aplicaciones con alta concurrencia, se recomienda implementar lógica de reintentos
        /// para DbUpdateConcurrencyException.
        /// </remarks>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                _logger.LogInformation("Guardando cambios en la base de datos");

                var affectedRecords = await _context.SaveChangesAsync();

                _logger.LogInformation("Cambios guardados exitosamente. Registros afectados: {Count}", affectedRecords);

                return affectedRecords;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Error de concurrencia al guardar los cambios");
                throw new DataAccessException("Error de concurrencia al guardar los cambios. " +
                    "Los datos pueden haber sido modificados por otro usuario.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de actualización en la base de datos al guardar los cambios");
                throw new DataAccessException("Error al actualizar la base de datos. " +
                    "Verifique las restricciones de integridad y validaciones.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al guardar los cambios");
                throw new DataAccessException("Error inesperado al guardar los cambios en la base de datos.", ex);
            }
        }
    }
    /// <summary>
    /// Excepción personalizada para errores de acceso a datos.
    /// Encapsula errores específicos de Entity Framework Core y proporciona contexto adicional.
    /// </summary>
    /// <remarks>
    /// Esta excepción se utiliza para envolver todos los errores de acceso a datos,
    /// permitiendo un manejo consistente de errores en capas superiores de la aplicación.
    /// Siempre incluye la excepción original como InnerException para facilitar el diagnóstico.
    /// </remarks>
    public class DataAccessException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de DataAccessException con un mensaje específico.
        /// </summary>
        /// <param name="message">Mensaje descriptivo del error.</param>
        public DataAccessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de DataAccessException con un mensaje y la excepción interna.
        /// </summary>
        /// <param name="message">Mensaje descriptivo del error.</param>
        /// <param name="innerException">Excepción original que causó este error.</param>
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}