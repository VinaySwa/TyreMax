using TyreSaleService.Models;

namespace TyreSaleService.Services
{
    public interface ITyreService
    {
        Task<IEnumerable<Tyre>> GetAllAsync();
        Task<Tyre> GetByIdAsync(int id);

        Task<IEnumerable<Tyre>> FindByDimensionsAsync(int width, int profile, int rimSize);
        Task<Tyre> CreateAsync(Tyre tyre);
        Task UpdateAsync(Tyre tyre);
        Task DeleteAsync(int id);
    }
}
