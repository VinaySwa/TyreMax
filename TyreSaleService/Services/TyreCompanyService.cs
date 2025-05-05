using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TyreSaleService.Common;
using TyreSaleService.Data;
using TyreSaleService.Models;
using TyreSaleService.Repository;

namespace TyreSaleService.Services
{
    /// <summary>
    /// Service class for managing tyre companies.
    /// </summary>
    public class TyreCompanyService : ITyreCompanyService
    {
        private readonly IUnitOfWork uow;
        private readonly AppDbContext dbContext;
        private readonly ILogger<TyreCompanyService> log;

        /// <summary>
        /// Constructor for TyreCompanyService.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public TyreCompanyService(
            IUnitOfWork unitOfWork,
            AppDbContext context,
            ILogger<TyreCompanyService> logger)
        {
            uow = unitOfWork;
            dbContext = context;
            log = logger;
        }

        /// <summary>
        /// Retrieves all tyre companies with their models and tyres.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<IEnumerable<TyreCompany>> GetAllAsync()
        {
            try
            {
                return await dbContext.TyreCompanies
                                     .Include(c => c.Models)
                                     .ThenInclude(m => m.Tyres)
                                     .ToListAsync();
            }
            catch (DbException dbEx)
            {
                log.LogError(dbEx, "Database error retrieving tyre companies with models");
                throw new DataAccessException("Could not retrieve tyre companies.", dbEx);
            }
        }

        /// <summary>
        /// Retrieves a tyre company by its ID, including its models and tyres.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<TyreCompany> GetByIdAsync(int id)
        {
            try
            {
                var company = await dbContext.TyreCompanies
                                            .Include(c => c.Models)
                                            .ThenInclude(m => m.Tyres)
                                            .FirstOrDefaultAsync(c => c.Id == id);
                if (company == null)
                {
                    log.LogWarning("TyreCompany with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"TyreCompany {id} not found.");
                }
                return company;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DbException dbEx)
            {
                log.LogError(dbEx, "Database error retrieving tyre company {Id} with models", id);
                throw new DataAccessException($"Could not retrieve tyre company {id}.", dbEx);
            }
        }

        /// <summary>
        /// Creates a new tyre company.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task<TyreCompany> CreateAsync(TyreCompany company)
        {
            try
            {
                await uow.TyreCompanies.AddAsync(company);
                await uow.SaveChangesAsync();
                return company;
            }
            catch (DbException dbEx)
            {
                log.LogError(dbEx, "Database error creating tyre company");
                throw new DataAccessException("Could not create tyre company.", dbEx);
            }
        }

        /// <summary>
        /// Updates an existing tyre company.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public async Task UpdateAsync(TyreCompany company)
        {
            try
            {
                uow.TyreCompanies.Update(company);
                await uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ccEx)
            {
                log.LogWarning(ccEx, "Concurrency error updating tyre company {Id}", company.Id);
                throw new DataAccessException(
                    $"Could not update tyre company {company.Id} due to a concurrency issue.",
                    ccEx);
            }
            catch (DbException dbEx)
            {
                log.LogError(dbEx, "Database error updating tyre company {Id}", company.Id);
                throw new DataAccessException(
                    $"Could not update tyre company {company.Id}.",
                    dbEx);
            }
        }

        /// <summary>
        /// Deletes a tyre company by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="DataAccessException"></exception>
        public async Task DeleteAsync(int id)
        {
            var entity = await uow.TyreCompanies.GetByIdAsync(id);
            if (entity == null)
            {
                log.LogWarning("Attempt to delete non-existent tyre company {Id}", id);
                throw new KeyNotFoundException($"TyreCompany {id} not found.");
            }
            try
            {
                uow.TyreCompanies.Delete(entity);
                await uow.SaveChangesAsync();
            }
            catch (DbException dbEx)
            {
                log.LogError(dbEx, "Database error deleting tyre company {Id}", id);
                throw new DataAccessException($"Could not delete tyre company {id}.", dbEx);
            }
        }
    }
}

