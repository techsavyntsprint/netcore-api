using APICore.API.Controllers;
using APICore.Data;
using APICore.Data.Entities;
using APICore.Data.Entities.Enums;
using APICore.Data.UoW;
using APICore.Services;
using APICore.Services.Exceptions;
using APICore.Services.Impls;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.WindowsAzure.Storage;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;
using Xunit;

namespace APICore.Tests.Integration.Account
{
    public class LogoutAction
    {
        private DbContextOptions<CoreDbContext> ContextOptions { get; }
        private readonly Mock<IConfiguration> Config;
        private readonly Mock<IDetectionService> DetectionService;
        private readonly CloudStorageAccount StorageAccount;

        public LogoutAction()
        {
            ContextOptions = new DbContextOptionsBuilder<CoreDbContext>()
                                                   .UseInMemoryDatabase("TestLogoutDatabase")
                                                   .Options;
            Config = new Mock<IConfiguration>();
            Config.Setup(setup => setup.GetSection("BearerTokens")["Issuer"]).Returns(@"http://apicore.com");
            Config.Setup(setup => setup.GetSection("BearerTokens")["Key"]).Returns(@"GUID-A54a-SS15-SwEr-opo4-56YH");
            Config.Setup(setup => setup.GetSection("BearerTokens")["Audience"]).Returns(@"Any");
            Config.Setup(setup => setup.GetSection("BearerTokens")["AccessTokenExpirationHours"]).Returns("7");
            Config.Setup(setup => setup.GetSection("BearerTokens")["RefreshTokenExpirationHours"]).Returns("60");

            DetectionService = new Mock<IDetectionService>();
            DetectionService.Setup(setup => setup.UserAgent).Returns(new UserAgent(@"Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 86.0) Gecko / 20100101 Firefox / 86.0"));

            StorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1/apicore;");

            SeedAsync().Wait();
        }
        private async Task SeedAsync()
        {
            await using var context = new CoreDbContext(ContextOptions);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Users.AddAsync(new User
            {
                Id = 2,
                Email = "carlos@itguy.com",
                FullName = "Carlos Delgado",
                Gender = 0,
                Phone = "+53 12345678",
                Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0=",
                Status = StatusEnum.ACTIVE
            });

            await context.SaveChangesAsync();
        }

        [Fact(DisplayName = "Successfully Logout Should Return Ok Status Code (200)")]
        public void SuccessfullyLogoutShouldReturnOk()
        {
            // ARRANGE
            var fakeClaims = new List<Claim>()
            {
                new (ClaimTypes.UserData, "2")
            };
            var identity = new ClaimsIdentity(fakeClaims, "Test");

            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claimsPrincipal)
            };

            httpContext.Request.Headers.Add("Authorization", @"Bearer s0m34cc$3$$T0k3n");

            using var context = new CoreDbContext(ContextOptions);

            var accountService = new AccountService(Config.Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, DetectionService.Object, StorageAccount.CreateCloudBlobClient());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            // ACT
            var taskResult = (OkResult)accountController.Logout().Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Wrong User Logout Should Return Not Found Exception(404)")]
        public void WrongUserLogoutShouldReturnNotFoundException()
        {
            // ARRANGE
            var fakeClaims = new List<Claim>()
            {
                new (ClaimTypes.UserData, "999999")
            };
            var identity = new ClaimsIdentity(fakeClaims, "Test");

            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claimsPrincipal)
            };

            httpContext.Request.Headers.Add("Authorization", @"Bearer s0m34cc$3$$T0k3n");

            using var context = new CoreDbContext(ContextOptions);

            var accountService = new AccountService(Config.Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, DetectionService.Object, StorageAccount.CreateCloudBlobClient());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            // ACT
            var aggregateException = accountController.GlobalLogout().Exception;
            var taskResult = (BaseNotFoundException)aggregateException?.InnerException;

            // ASSERT
            if (taskResult != null) Assert.Equal(404, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Successfully Global Logout Should Return Ok Status Code (200)")]
        public void SuccessfullyGlobalLogoutShouldReturnOk()
        {
            // ARRANGE
            var fakeClaims = new List<Claim>()
            {
                new (ClaimTypes.UserData, "2")
            };
            var identity = new ClaimsIdentity(fakeClaims, "Test");

            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claimsPrincipal)
            };

            httpContext.Request.Headers.Add("Authorization", @"Bearer s0m34cc$3$$T0k3n");

            using var context = new CoreDbContext(ContextOptions);

            var accountService = new AccountService(Config.Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, DetectionService.Object, StorageAccount.CreateCloudBlobClient());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            // ACT
            var taskResult = (OkResult)accountController.GlobalLogout().Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Wrong User Global Logout Should Return Not Found Exception(404)")]
        public void WrongUserGlobalLogoutShouldReturnNotFoundException()
        {
            // ARRANGE
            var fakeClaims = new List<Claim>()
            {
                new (ClaimTypes.UserData, "999999")
            };
            var identity = new ClaimsIdentity(fakeClaims, "Test");

            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(claimsPrincipal)
            };

            httpContext.Request.Headers.Add("Authorization", @"Bearer s0m34cc$3$$T0k3n");

            using var context = new CoreDbContext(ContextOptions);

            var accountService = new AccountService(Config.Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, DetectionService.Object, StorageAccount.CreateCloudBlobClient());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            // ACT
            var aggregateException = accountController.GlobalLogout().Exception;
            var taskResult = (BaseNotFoundException)aggregateException?.InnerException;

            // ASSERT
            if (taskResult != null) Assert.Equal(404, taskResult.HttpCode);
        }
    }
}
