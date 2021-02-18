using Moq;
using System;
using Xunit;
using APICore.Services;
using APICore.Common.DTO.Request;
using APICore.API.Controllers;
using Microsoft.AspNetCore.Hosting;
using APICore.Data.UoW;
using APICore.Services.Impls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Wangkanai.Detection.Services;
using System.Collections.Generic;
using APICore.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using APICore.Services.Exceptions;
using APICore.Data.Repository;
using APICore.Data;
using Microsoft.EntityFrameworkCore;
using Wangkanai.Detection.Models;
using Microsoft.AspNetCore.Http;

namespace APICore.Test
{
    public class AccountTesting
    {
        private readonly Mock<IUnitOfWork> uowMock;

        public AccountTesting()
        {
            uowMock = new Mock<IUnitOfWork>();
        }

        [Fact(DisplayName = "Successfully Register Should Return Created Status Code (201)")]
        public void SuccessfullyRegisterShouldReturnCreated()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "Carlos@itguy.com",
                FullName = "Carlos Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "S3cretP@$$",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountService = new AccountService(new Mock<IConfiguration>().Object, Setup.MockDatabase().Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
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
                Password = "S3cretP@$$",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountService = new AccountService(new Mock<IConfiguration>().Object, Setup.MockDatabase().Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Email In Use Should Return Bad Request Exception")]
        public void EmailInUseShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe@itguy.com"
            };

            var accountService = new AccountService(new Mock<IConfiguration>().Object, Setup.MockDatabase().Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Empty Password Should Return Bad Request Exception")]
        public void EmptyPasswordShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe2@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountService = new AccountService(new Mock<IConfiguration>().Object, Setup.MockDatabase().Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);


            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
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
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountService = new AccountService(new Mock<IConfiguration>().Object, Setup.MockDatabase().Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);


            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
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
                Password = "Z3cretP@$$",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountService = new AccountService(new Mock<IConfiguration>().Object, Setup.MockDatabase().Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Successfully Login Should Return Ok Status Code (200)")]
        public void SuccessfullyLoginShouldReturnOk()
        {
            // ARRANGE
            var fakeLoginRequest = new LoginRequest
            {
                Email = "carlos@itguy.com",
                Password = "S3cretP@$$"
            };

            var ds = new Mock<IDetectionService>();
            ds.Setup(setup => setup.UserAgent).Returns(new UserAgent(@"Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 86.0) Gecko / 20100101 Firefox / 86.0"));

            // Necessary for login  
            var config = new Mock<IConfiguration>();
            config.Setup(setup => setup.GetSection("BearerTokens")["Issuer"]).Returns(@"http://apicore.com");
            config.Setup(setup => setup.GetSection("BearerTokens")["Key"]).Returns(@"GUID-A54a-SS15-SwEr-opo4-56YH");
            config.Setup(setup => setup.GetSection("BearerTokens")["Audience"]).Returns(@"Any");
            config.Setup(setup => setup.GetSection("BearerTokens")["AccessTokenExpirationHours"]).Returns("7");
            config.Setup(setup => setup.GetSection("BearerTokens")["RefreshTokenExpirationHours"]).Returns("60");

            var httpContext = new DefaultHttpContext();

            var accountService = new AccountService(config.Object, Setup.MockDatabase().Object, new Mock<IStringLocalizer<IAccountService>>().Object, ds.Object, Setup.GetBlobHardCodedSettings());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object) {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                }
            };

            // ACT
            var taskResult = (ObjectResult)accountController.Login(fakeLoginRequest).Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }
    }
}
