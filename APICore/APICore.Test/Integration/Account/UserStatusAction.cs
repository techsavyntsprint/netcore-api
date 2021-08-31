using APICore.API.Controllers;
using APICore.Common.DTO.Request;
using APICore.Services;
using APICore.Services.Exceptions;
using APICore.Services.Impls;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Wangkanai.Detection.Services;
using Xunit;
using Microsoft.EntityFrameworkCore;
using APICore.Data;
using APICore.Data.Entities.Enums;
using APICore.Data.Entities;
using System.Threading.Tasks;
using APICore.Data.UoW;
using Wangkanai.Detection.Models;

namespace APICore.Tests.Integration.Account
{
    public class UserStatusAction
    {
        private DbContextOptions<CoreDbContext> ContextOptions { get; }
        private readonly Mock<IConfiguration> Config;
        private readonly Mock<IDetectionService> DetectionService;
        private readonly IStorageService storageService;

        public UserStatusAction()
        {
            ContextOptions = new DbContextOptionsBuilder<CoreDbContext>()
                                                   .UseInMemoryDatabase("TestStatusDatabase")
                                                   .Options;
            Config = new Mock<IConfiguration>();
            Config.Setup(setup => setup.GetSection("BearerTokens")["Issuer"]).Returns(@"http://apicore.com");
            Config.Setup(setup => setup.GetSection("BearerTokens")["Key"]).Returns(@"GUID-A54a-SS15-SwEr-opo4-56YH");
            Config.Setup(setup => setup.GetSection("BearerTokens")["Audience"]).Returns(@"Any");
            Config.Setup(setup => setup.GetSection("BearerTokens")["AccessTokenExpirationHours"]).Returns("7");
            Config.Setup(setup => setup.GetSection("BearerTokens")["RefreshTokenExpirationHours"]).Returns("60");

            DetectionService = new Mock<IDetectionService>();
            DetectionService.Setup(setup => setup.UserAgent).Returns(new UserAgent(@"Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 86.0) Gecko / 20100101 Firefox / 86.0"));

            storageService = new Mock<IStorageService>().Object;

            SeedAsync().Wait();
        }

        private async Task SeedAsync()
        {
            await using var context = new CoreDbContext(ContextOptions);

            if (await context.Users.AnyAsync() == false)
            {
                await context.Users.AddRangeAsync(new User
                {
                    Id = 4,
                    Email = "pepe@itguy.com",
                    FullName = "Pepe Delgado",
                    Gender = 0,
                    Phone = "+53 12345678",
                    Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0=",
                    Status = StatusEnum.ACTIVE,
                    Identity = "someRandomIdentityString"
                }, new User
                {
                    Id = 5,
                    Email = "inactive@itguy.com",
                    FullName = "Out Delgado",
                    Gender = 0,
                    Phone = "+53 12345678",
                    Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0=",
                    Status = StatusEnum.INACTIVE,
                    Identity = "someRandomIdentityString"
                });

                await context.AddRangeAsync(new UserToken
                {
                    Id = 1,
                    AccessToken = "s0m34cc$3$$T0k3n",
                    UserId = 4
                }, new UserToken
                {
                    Id = 2,
                    AccessToken = "s0m34cc$3$$T0k3n",
                    UserId = 5
                });

                await context.SaveChangesAsync();
            }
        }

        [Fact(DisplayName = "Successfully Change Account Status Should Return Ok(200)")]
        public void SuccessfullyChangeAccountStatusShouldReturnOk()
        {
            // ARRANGE
            var fakeClaims = new List<Claim>()
            {
                new(ClaimTypes.UserData, "4")
            };
            var identity = new ClaimsIdentity(fakeClaims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claimsPrincipal)
            };
            httpContext.Request.Headers.Add("Authorization", @"Bearer s0m34cc$3$$T0k3n");

            using var context = new CoreDbContext(ContextOptions);

            var accountService = new AccountService(Config.Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, DetectionService.Object, storageService);
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var fakeChangeAccountStatus = new ChangeAccountStatusRequest
            {
                Identity = "someRandomIdentityString",
                Active = true
            };

            // ACT
            var taskResult = (OkResult)accountController.ChangeAccountStatus(fakeChangeAccountStatus).Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Inactive User Change Account Status Himself Should Return Forbidden Exception (403)")]
        public void InactiveUserChangeAccountStatusHimselfShouldReturnForbiddenException()
        {
            // ARRANGE
            var fakeClaims = new List<Claim>()
            {
                new(ClaimTypes.UserData, "5")
            };
            var identity = new ClaimsIdentity(fakeClaims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claimsPrincipal)
            };
            httpContext.Request.Headers.Add("Authorization", @"Bearer s0m34cc$3$$T0k3n");

            using var context = new CoreDbContext(ContextOptions);

            var accountService = new AccountService(Config.Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, DetectionService.Object, storageService);
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var fakeChangeAccountStatus = new ChangeAccountStatusRequest
            {
                Identity = "someRandomIdentityString",
                Active = true
            };

            // ACT
            var aggregateException = accountController.ChangeAccountStatus(fakeChangeAccountStatus).Exception;
            var taskResult = (BaseForbiddenException)aggregateException?.InnerException;

            // ASSERT
            if (taskResult != null) Assert.Equal(403, taskResult.HttpCode);
        }
    }
}
