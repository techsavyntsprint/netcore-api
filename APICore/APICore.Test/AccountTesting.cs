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
                Email = "pepe@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "S3cretP@$$",
                ConfirmationPassword = "S3cretP@$$"
            };

            var users = new List<User>();
            uowMock.Setup(repo => repo.UserRepository.FindAllAsync(x => x.Email == fakeUserRequest.Email)).ReturnsAsync(users);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, uowMock.Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
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

            var users = new List<User>();
            uowMock.Setup(repo => repo.UserRepository.FindAllAsync(x => x.Email == fakeUserRequest.Email)).ReturnsAsync(users);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, uowMock.Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
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
                Email = "carlos22@itguy.com"
            };

            var users = Setup.FakeUsersData();
            uowMock.Setup(repo => repo.UserRepository.FindAllAsync(x => x.Email == fakeUserRequest.Email)).ReturnsAsync(users);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, uowMock.Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
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
                Email = "pepe@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "",
                ConfirmationPassword = "S3cretP@$$"
            };

            var users = new List<User>();
            uowMock.Setup(repo => repo.UserRepository.FindAllAsync(x => x.Email == fakeUserRequest.Email)).ReturnsAsync(users);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, uowMock.Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
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
                Email = "pepe@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "S3cr",
                ConfirmationPassword = "S3cretP@$$"
            };

            var users = new List<User>();
            uowMock.Setup(repo => repo.UserRepository.FindAllAsync(x => x.Email == fakeUserRequest.Email)).ReturnsAsync(users);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, uowMock.Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
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
                Email = "pepe@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "Z3cretP@$$",
                ConfirmationPassword = "S3cretP@$$"
            };

            var users = new List<User>();
            uowMock.Setup(repo => repo.UserRepository.FindAllAsync(x => x.Email == fakeUserRequest.Email)).ReturnsAsync(users);
            var accountService = new AccountService(new Mock<IConfiguration>().Object, uowMock.Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, Setup.GetBlobHardCodedSettings());
            var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);


            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }
    }
}
