using System;
using System.Threading.Tasks;
using APICore.Data;
using APICore.Data.Entities;
using APICore.Data.UoW;
using APICore.Services.Exceptions;
using APICore.Services.Impls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace APICore.Services.Tests.SettingServiceTests
{
    public abstract class SettingServiceTests
    {
        protected DbContextOptions<CoreDbContext> ContextOptions { get; }

        private readonly IStringLocalizer<IAccountService> _localizerMock;

        protected SettingServiceTests(DbContextOptions<CoreDbContext> contextOptions)
        {
            ContextOptions = contextOptions ?? throw new ArgumentNullException(nameof(contextOptions));

            _localizerMock = new Mock<IStringLocalizer<IAccountService>>().Object;

            SeedAsync()
               .Wait();
        }

        #region Seeding

        private async Task SeedAsync()
        {
            await using var context = new CoreDbContext(ContextOptions);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Setting.AddRangeAsync(new Setting
            {
                Key = "TestKey1",
                Value = "1"
            }, new Setting
            {
                Key = "TestKey2",
                Value = "2"
            });

            await context.SaveChangesAsync();
        }

        #endregion

        #region GetSetting with not existing key throws not found exception

        [Fact]
        public async Task GetSetting_NotExistingKey_ThrowsNotFound()
        {
            // Arrange
            const string notExistingKey = "TestBadKey";

            await using var context = new CoreDbContext(ContextOptions);
            var service = new SettingService(new UnitOfWork(context), _localizerMock);

            // Act
            var getSettingAsync = service.GetSettingAsync(notExistingKey);

            // Assert
            await Assert.ThrowsAsync<SettingNotFoundException>(() => getSettingAsync);
        }

        #endregion

        #region GetSetting with existing key returns value

        [Fact]
        public async Task GetSetting_ExistingKey_ReturnsValue()
        {
            // Arrange
            const string existingKey = "TestKey1";

            await using var context = new CoreDbContext(ContextOptions);
            var service = new SettingService(new UnitOfWork(context), _localizerMock);

            // Act
            var value = await service.GetSettingAsync(existingKey);

            // Assert
            Assert.Equal("1", value);
        }

        #endregion
    }
}
