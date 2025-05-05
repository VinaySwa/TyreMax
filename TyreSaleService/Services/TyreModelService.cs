using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TyreSaleService.Common;
using TyreSaleService.Data;
using TyreSaleService.Models;
using TyreSaleService.Repository;

namespace TyreSaleService.Services
{
    /// <summary>
    /// Service class for managing tyre models.
    /// </summary>
    public class TyreModelService : ITyreModelService
    {
        private readonly IUnitOfWork uow;
        private readonly AppDbContext dbContext;
        private readonly ILogger<TyreModelService> log;

        /// <summary>
        /// Constructor for TyreModelService.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public TyreModelService(IUnitOfWork unitOfWork, AppDbContext context, ILogger<TyreModelService> logger)
        {
            uow = unitOfWork;
            dbContext = context;
            log = logger;
        }

        /// <summary>
        /// Retrieves all tyre models with their associated tyres.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<IEnumerable<TyreModel>> GetAllAsync()
        {
            try
            {
                return await dbContext.TyreModels
                                     .Include(m => m.Tyres)
                                     .ToListAsync();
            }
            catch (DbException dbEx)
            {
                log.LogError(dbEx, "Database error retrieving all tyre models");
                throw new DataAccessException("Could not retrieve tyre models.", dbEx);
            }
        }

        /// <summary>
        /// Retrieves a tyre model by its ID, including its associated tyres.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<TyreModel> GetByIdAsync(int id)
        {
            var model = await dbContext.TyreModels
                                          .Include(m => m.Tyres)
                                          .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                log.LogWarning($"TyreModel with ID {id} not found.");
                throw new KeyNotFoundException($"TyreModel {id} not found.");
            }
            return model;
        }

        /// <summary>
        /// Creates a new tyre model and saves it to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<TyreModel> CreateAsync(TyreModel model)
        {
            try
            {
                await uow.TyreModels.AddAsync(model);
                await uow.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException dbEx)
            {
                log.LogError(dbEx, "Database error creating tyre model");
                throw new DataAccessException("Could not create tyre model.", dbEx);
            }
        }

        /// <summary>
        /// Updates an existing tyre model in the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task UpdateAsync(TyreModel model)
        {
            try
            {
                uow.TyreModels.Update(model);
                await uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ccEx)
            {
                log.LogWarning(ccEx, $"Concurrency error updating tyre model {model.Id}");
                throw new DataAccessException($"Could not update tyre model {model.Id} due to a concurrency issue.", ccEx);
            }
            catch (DbUpdateException dbEx)
            {
                log.LogError(dbEx, $"Database error updating tyre model {model.Id}");
                throw new DataAccessException($"Could not update tyre model {model.Id}.", dbEx);
            }
        }

        /// <summary>
        /// Deletes a tyre model by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="DataAccessException"></exception>
        public async Task DeleteAsync(int id)
        {
            var entity = await uow.TyreModels.GetByIdAsync(id);
            if (entity == null)
            {
                log.LogWarning($"Attempt to delete non-existent tyre model {id}");
                throw new KeyNotFoundException($"TyreModel {id} not found.");
            }
            try
            {
                uow.TyreModels.Delete(entity);
                await uow.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                log.LogError(dbEx, $"Database error deleting tyre model {id}");
                throw new DataAccessException($"Could not delete tyre model {id}.", dbEx);
            }
        }
    }
}
