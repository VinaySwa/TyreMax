using TyreSaleService.Data;
using TyreSaleService.Models;

namespace TyreSaleService.Repository
{
    /// <summary>
    /// Unit of Work class for managing multiple repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IGenericRepository<TyreCompany> TyreCompanies { get; }
        public IGenericRepository<TyreModel> TyreModels { get; }
        public IGenericRepository<Tyre> Tyres { get; }

        /// <summary>
        /// Constructor for UnitOfWork.
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            TyreCompanies = new GenericRepository<TyreCompany>(_context);
            TyreModels = new GenericRepository<TyreModel>(_context);
            Tyres = new GenericRepository<Tyre>(_context);
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        /// <summary>
        /// Disposes the UnitOfWork and its context.
        /// </summary>
        public void Dispose() =>
            _context.Dispose();
    }
}
