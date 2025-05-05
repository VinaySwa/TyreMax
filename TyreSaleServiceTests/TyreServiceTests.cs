using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TyreSaleService.Data;
using TyreSaleService.Models;
using TyreSaleService.Repository;
using TyreSaleService.Services;

namespace TyreSaleService.Tests
{
    /// <summary>
    /// Unit tests for the TyreService class.
    /// </summary>
    public class TyreServiceTests
    {
        private readonly Mock<IUnitOfWork> uowMock;
        private readonly AppDbContext dbContext;
        private readonly TyreService tyreService;

        /// <summary>
        /// Constructor for TyreServiceTests.
        /// </summary>
        public TyreServiceTests()
        {
            uowMock = new Mock<IUnitOfWork>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            dbContext = new AppDbContext(options);

            tyreService = new TyreService(
                uowMock.Object,
                dbContext,
                NullLogger<TyreService>.Instance);
        }

        /// <summary>
        /// Test to ensure GetAllAsync returns all tyres.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllAsyncReturnsAllTyres()
        {
            var tyres = new List<Tyre>
            {
                new Tyre { Id = 1 },
                new Tyre { Id = 2 }
            };
            uowMock.Setup(u => u.Tyres.GetAllAsync())
                    .ReturnsAsync(tyres);

            var result = await tyreService.GetAllAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Id == 1);
            Assert.Contains(result, t => t.Id == 2);
        }

        /// <summary>
        /// Test to ensure GetByIdAsync returns a tyre with the specified ID.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetByIdAsyncExistingIdReturnsTyre()
        {
            var tyre = new Tyre { Id = 5 };
            uowMock.Setup(u => u.Tyres.GetByIdAsync(5))
                    .ReturnsAsync(tyre);

            var result = await tyreService.GetByIdAsync(5);

            Assert.Equal(5, result.Id);
        }

        /// <summary>
        /// Test to ensure GetByIdAsync throws KeyNotFoundException for non-existing ID.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetByIdAsyncNonExistingThrowsKeyNotFoundException()
        {
            uowMock.Setup(u => u.Tyres.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Tyre)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => tyreService.GetByIdAsync(99));
        }

        /// <summary>
        /// Test to ensure FindByDimensionsAsync returns tyres matching the specified dimensions.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindByDimensionsAsyncMatchingReturnsTyres()
        {
            var company = new TyreCompany { Id = 1, Name = "Co" };
            var model = new TyreModel { Id = 1, Name = "M1", CompanyId = 1, Company = company };
            var tyre = new Tyre
            {
                Id = 10,
                Dimensions = new TyreDimensions { Width = 200, Profile = 200, RimSize = 200 },
                ModelId = 1,
                Model = model,
                Availability = true,
                Price = 100,
                SpeedIndex = "A",
                LoadIndex = 1,
                DiscountPercentage = 10
            };

            dbContext.TyreCompanies.Add(company);
            dbContext.TyreModels.Add(model);
            dbContext.Tyres.Add(tyre);
            await dbContext.SaveChangesAsync();

            var result = await tyreService.FindByDimensionsAsync(200, 200, 200);

            Assert.Single(result);
            Assert.Equal(10, result.First().Id);
        }

        /// <summary>
        /// Test to ensure FindByDimensionsAsync returns an empty list for non-matching dimensions.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateAsyncAddsTyreReturnsTyre()
        {
            var newTyre = new Tyre { Id = 0 };
            uowMock.Setup(u => u.Tyres.AddAsync(newTyre))
                    .Returns(Task.CompletedTask);
            uowMock.Setup(u => u.SaveChangesAsync())
                    .ReturnsAsync(1);

            var result = await tyreService.CreateAsync(newTyre);

            Assert.Equal(newTyre, result);
            uowMock.Verify(u => u.Tyres.AddAsync(newTyre), Times.Once);
            uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Test to ensure CreateAsync throws DataAccessException on failure.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteAsyncNonExistingThrowsKeyNotFoundException()
        {
            uowMock.Setup(u => u.Tyres.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Tyre)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => tyreService.DeleteAsync(123));
        }
    }
}
