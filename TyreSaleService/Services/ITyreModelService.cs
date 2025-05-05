using TyreSaleService.Models;

namespace TyreSaleService.Services
{
    public interface ITyreModelService
    {
        Task<IEnumerable<TyreModel>> GetAllAsync();
        Task<TyreModel> GetByIdAsync(int id);
        Task<TyreModel> CreateAsync(TyreModel model);
        Task UpdateAsync(TyreModel model);
        Task DeleteAsync(int id);
    }
}
