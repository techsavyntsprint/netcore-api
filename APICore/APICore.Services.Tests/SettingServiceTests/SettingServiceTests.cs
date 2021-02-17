﻿using System;
using System.Threading.Tasks;
using APICore.Data;
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
        private readonly IStringLocalizer<IAccountService> _localizerMock;
        private readonly DbContextOptions<CoreDbContext> _dbContextOptions;

        protected SettingServiceTests(DbContextOptions<CoreDbContext> contextOptions)
        {
            _dbContextOptions = contextOptions ?? throw new ArgumentNullException(nameof(contextOptions));

            _localizerMock = new Mock<IStringLocalizer<IAccountService>>().Object;

            SeedAsync().Wait();
        }

        #region Seeding

        private async Task SeedAsync()
        {
            await using var context = new CoreDbContext(_dbContextOptions);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.SaveChangesAsync();
        }

        #endregion

        #region GetSetting on not existing key throws not found exception

        [Fact]
        public async Task GetSetting_NotExistingKey_ThrowsNotFound()
        {
            // Arrange
            const string notExistingKey = "TestKey";

            await using var context = new CoreDbContext(_dbContextOptions);
            var service = new SettingService(new UnitOfWork(context), _localizerMock);

            // Act
            var getSettingAsync = service.GetSettingAsync(notExistingKey);

            // Assert
            await Assert.ThrowsAsync<SettingNotFoundException>(() => getSettingAsync);
        }

        #endregion
    }
}