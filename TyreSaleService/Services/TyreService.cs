using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TyreSaleService.Common;
using TyreSaleService.Data;
using TyreSaleService.Models;
using TyreSaleService.Repository;

namespace TyreSaleService.Services
{
    /// <summary>
    /// Service class for managing tyre data.
    /// </summary>
    public class TyreService : ITyreService
    {
        private readonly IUnitOfWork _uow;
        private readonly AppDbContext dbContext;
        private readonly ILogger<TyreService> _logger;

        /// <summary>
        /// Constructor for TyreService.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public TyreService(
            IUnitOfWork unitOfWork,
            AppDbContext context,
            ILogger<TyreService> logger)
        {
            _uow = unitOfWork;
            dbContext = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all tyres with their associated models and companies.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<IEnumerable<Tyre>> GetAllAsync()
        {
            try
            {
                return await _uow.Tyres.GetAllAsync();
            }
            catch (DbException dbEx)
            {
                _logger.LogError(dbEx, "Database error retrieving all tyres");
                throw new DataAccessException("Could not retrieve tyres.", dbEx);
            }
        }

        /// <summary>
        /// Retrieves a tyre by its ID, including its associated model and company.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Tyre> GetByIdAsync(int id)
        {
            var tyre = await _uow.Tyres.GetByIdAsync(id);
            if (tyre == null)
            {
                _logger.LogWarning("Tyre with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Tyre {id} not found.");
            }
            return tyre;
        }

        /// <summary>
        /// Retrieves tyres filtered by their dimensions (width, profile, rim size).
        /// </summary>
        /// <param name="width"></param>
        /// <param name="profile"></param>
        /// <param name="rimSize"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<IEnumerable<Tyre>> FindByDimensionsAsync(int width, int profile, int rimSize)
        {
            try
            {
                IQueryable<Tyre> query = dbContext.Tyres
                    .Where(t =>
                        t.Dimensions.Width == width &&
                        t.Dimensions.Profile == profile &&
                        t.Dimensions.RimSize == rimSize)
                    .Include(t => t.Model)
                        .ThenInclude(m => m.Company);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering tyres by dimensions {Width}/{Profile}R{RimSize}", width, profile, rimSize);
                throw new DataAccessException("Could not filter tyres by dimensions.", ex);
            }
        }

        /// <summary>
        /// Creates a new tyre and saves it to the database.
        /// </summary>
        /// <param name="tyre"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<Tyre> CreateAsync(Tyre tyre)
        {
            try
            {
                await _uow.Tyres.AddAsync(tyre);
                await _uow.SaveChangesAsync();
                return tyre;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error creating tyre");
                throw new DataAccessException("Could not create tyre.", dbEx);
            }
        }

        /// <summary>
        /// Updates an existing tyre in the database.
        /// </summary>
        /// <param name="tyre"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task UpdateAsync(Tyre tyre)
        {
            try
            {
                _uow.Tyres.Update(tyre);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ccEx)
            {
                _logger.LogWarning(ccEx, "Concurrency error updating tyre {Id}", tyre.Id);
                throw new DataAccessException($"Could not update tyre {tyre.Id} due to a concurrency issue.", ccEx);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error updating tyre {Id}", tyre.Id);
                throw new DataAccessException($"Could not update tyre {tyre.Id}.", dbEx);
            }
        }

        /// <summary>
        /// Deletes a tyre by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="DataAccessException"></exception>
        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.Tyres.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Attempt to delete non-existent tyre {Id}", id);
                throw new KeyNotFoundException($"Tyre {id} not found.");
            }
            try
            {
                _uow.Tyres.Delete(entity);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error deleting tyre {Id}", id);
                throw new DataAccessException($"Could not delete tyre {id}.", dbEx);
            }
        }
    }
}

