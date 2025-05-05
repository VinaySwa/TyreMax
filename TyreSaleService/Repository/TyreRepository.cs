using Microsoft.EntityFrameworkCore;
using TyreSaleService.Data;
using TyreSaleService.Models;

namespace TyreSaleService.Repository
{
    /// <summary>
    /// Repository class for managing tyre data.
    /// </summary>
    public class TyreRepository : GenericRepository<Tyre>, ITyreRepository
    {
        public TyreRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Get only the tyres currently marked as available.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Tyre>> GetAvailableAsync() =>
            await dbContext.Set<Tyre>()
                          .Where(t => t.Availability)
                          .Include(t => t.Model)
                            .ThenInclude(m => m.Company)
                          .ToListAsync();

        /// <summary>
        /// Find tyres by their dimensions (width, profile, rim size).
        /// </summary>
        /// <param name="width"></param>
        /// <param name="profile"></param>
        /// <param name="rimSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Tyre>> FindBySizeAsync(int width, int profile, int rimSize) =>
            await dbContext.Set<Tyre>()
                          .Where(t => t.Dimensions.Width == width
                                   && t.Dimensions.Profile == profile
                                   && t.Dimensions.RimSize == rimSize)
                          .Include(t => t.Model)
                            .ThenInclude(m => m.Company)
                          .ToListAsync();
    }
}
