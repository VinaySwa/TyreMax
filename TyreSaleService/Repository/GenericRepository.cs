using Microsoft.EntityFrameworkCore;
using TyreSaleService.Data;

namespace TyreSaleService.Repository
{
    /// <summary>
    /// Generic repository interface for CRUD operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext dbContext;

        /// <summary>
        /// Constructor for the generic repository.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(AppDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Retrieves all entities of type T.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }
    }
}
