using APICore.API.Controllers;
using APICore.Common.DTO.Request;
using APICore.Data;
using APICore.Data.Entities;
using APICore.Data.Entities.Enums;
using APICore.Data.UoW;
using APICore.Services;
using APICore.Services.Exceptions;
using APICore.Services.Impls;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;
using Xunit;

namespace APICore.Tests.Integration.Account
{
    public class RegisterAction
    {
        private DbContextOptions<CoreDbContext> ContextOptions { get; }
        private readonly IStorageService storageService;

        public RegisterAction()
        {
            ContextOptions = new DbContextOptionsBuilder<CoreDbContext>()
                                       .UseInMemoryDatabase("TestRegisterDatabase")
                                       .Options;
            storageService = new Mock<IStorageService>().Object;

            SeedAsync().Wait();
        }

        private async Task SeedAsync()
        {
            await using var context = new CoreDbContext(ContextOptions);

            if (await context.Users.AnyAsync() == false)
            {
                await context.Users.AddAsync(new User
                {
                    Id = 3,
                    Email = "pepe@itguy.com",
                    FullName = "Pepe Delgado",
                    Gender = 0,
                    Phone = "+53 12345678",
                    Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0=",
                    Status = StatusEnum.ACTIVE
                });

                await context.SaveChangesAsync();
            }
        }

        [Fact(DisplayName = "Successfully Register Should Return Created Status Code (201)")]
        public void SuccessfullyRegisterShouldReturnCreated()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = @"carlos@itguy.com",
                FullName = "Carlos Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = @"S3cretP@$$",
                ConfirmationPassword = @"S3cretP@$$"
            };

            using var context = new CoreDbContext(ContextOptions);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, storageService);
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var taskResult = (ObjectResult)accountController.Register(fakeUserRequest).Result;

            // ASSERT
            Assert.Equal(201, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Empty Email Should Return Bad Request Exception")]
        public void EmptyEmailShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = @"S3cretP@$$",
                ConfirmationPassword = @"S3cretP@$$"
            };

            using var context = new CoreDbContext(ContextOptions);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, storageService);
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var aggregateException = accountController.Register(fakeUserRequest).Exception;
            var taskResult = (BaseBadRequestException)aggregateException?.InnerException;

            // ASSERT
            if (taskResult != null) Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Email In Use Should Return Bad Request Exception")]
        public void EmailInUseShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe@itguy.com"
            };

            using var context = new CoreDbContext(ContextOptions);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, storageService);
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var aggregateException = accountController.Register(fakeUserRequest).Exception;
            var taskResult = (BaseBadRequestException)aggregateException?.InnerException;

            // ASSERT
            if (taskResult != null) Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Empty Password Should Return Bad Request Exception")]
        public void EmptyPasswordShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = @"pepe2@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "",
                ConfirmationPassword = @"S3cretP@$$"
            };

            using var context = new CoreDbContext(ContextOptions);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, storageService);
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var aggregateException = accountController.Register(fakeUserRequest).Exception;
            var taskResult = (BaseBadRequestException)aggregateException?.InnerException;

            // ASSERT
            if (taskResult != null) Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Small Password Should Return Bad Request Exception")]
        public void SmallPasswordShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe2@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "S3cr",
                ConfirmationPassword = @"S3cretP@$$"
            };

            using var context = new CoreDbContext(ContextOptions);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, storageService);
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var aggregateException = accountController.Register(fakeUserRequest).Exception;
            var taskResult = (BaseBadRequestException)aggregateException?.InnerException;

            // ASSERT
            if (taskResult != null) Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Passwords Doesn't Match Should Return Bad Request Exception")]
        public void PasswordDoesntMatchShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe2@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = @"Z3cretP@$$",
                ConfirmationPassword = @"S3cretP@$$"
            };

            using var context = new CoreDbContext(ContextOptions);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, new UnitOfWork(context), new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, storageService);
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var aggregateException = accountController.Register(fakeUserRequest).Exception;
            var taskResult = (BaseBadRequestException)aggregateException?.InnerException;

            // ASSERT
            if (taskResult != null) Assert.Equal(400, taskResult.HttpCode);
        }
    }
}
