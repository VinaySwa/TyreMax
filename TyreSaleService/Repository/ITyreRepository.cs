using TyreSaleService.Models;

namespace TyreSaleService.Repository
{
    public interface ITyreRepository : IGenericRepository<Tyre>
    {
        /// <summary>
        /// Get only the tyres currently marked as available.
        /// </summary>
        Task<IEnumerable<Tyre>> GetAvailableAsync();

        /// <summary>
        /// Find tyres matching a specific dimension.
        /// </summary>
        Task<IEnumerable<Tyre>> FindBySizeAsync(int width, int profile, int rimSize);
    }
}
