using TyreSaleService.Models;

namespace TyreSaleService.Services
{
    public interface ITyreCompanyService
    {
        Task<IEnumerable<TyreCompany>> GetAllAsync();
        Task<TyreCompany> GetByIdAsync(int id);
        Task<TyreCompany> CreateAsync(TyreCompany company);
        Task UpdateAsync(TyreCompany company);
        Task DeleteAsync(int id);
    }
}
