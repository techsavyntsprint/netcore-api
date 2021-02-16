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
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using System.Collections.Generic;
using APICore.Data.Entities;
using APICore.Data.Repository;
using APICore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace APICore.Test
{
    public class AccountTesting
    {
        readonly Mock<IUnitOfWork> uowMock;
        readonly IList<User> users;
        readonly AccountController accountController; 

        public AccountTesting()
        {
            // Setup all the Mocks
            uowMock = new Mock<IUnitOfWork>();
            // Empty Repository of Users, populate on test if needed.
            users = new List<User>();
            uowMock.Setup(repo => repo.UserRepository.GetAllAsync()).ReturnsAsync(users);
            var configurationMock = new Mock<IConfiguration>().Object;
            var localizerMock = new Mock<IStringLocalizer<IAccountService>>().Object;
            var detectionServiceMock = new Mock<IDetectionService>().Object;
            var accountService = new AccountService(configurationMock, uowMock.Object, localizerMock, detectionServiceMock, GetBlobHardCodedSettings());
            var autoMapperMock = new Mock<AutoMapper.IMapper>().Object;
            var emailServiceMock = new Mock<IEmailService>().Object;
            var webHostingEnvMock = new Mock<IWebHostEnvironment>().Object;
            accountController = new AccountController(accountService, autoMapperMock, emailServiceMock, webHostingEnvMock);
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

            // ACT
            var taskResult = (ObjectResult)accountController.Register(fakeUserRequest).Result;

            // ASSERT
            Assert.Equal(201, taskResult.StatusCode);
        }
        //BLOB Setting are needed and for now they are hardcodded.
        internal CloudBlobClient GetBlobHardCodedSettings()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1/apicore;");
            return storageAccount.CreateCloudBlobClient();
        }
    }
}
