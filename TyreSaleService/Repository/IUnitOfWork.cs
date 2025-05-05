using TyreSaleService.Models;

namespace TyreSaleService.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TyreCompany> TyreCompanies { get; }
        IGenericRepository<TyreModel> TyreModels { get; }
        IGenericRepository<Tyre> Tyres { get; }
        Task<int> SaveChangesAsync();
    }
}
