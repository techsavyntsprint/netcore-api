using APICore.API.Controllers;
using APICore.Services;
using APICore.Services.Impls;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;

namespace APICore.Test.Mocks
{
    class LoginAction
    {
        public static AccountController LoginEndpoint
        {
            get
            {
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

                var accountService = new AccountService(config.Object, new DBContext().MockRepository().Object, new Mock<IStringLocalizer<IAccountService>>().Object, ds.Object, GetBlobHardCodedSettings());
                var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object)
                {
                    ControllerContext = new ControllerContext()
                    {
                        HttpContext = httpContext
                    }
                };

                return accountController;
            }
        }

        private static CloudBlobClient GetBlobHardCodedSettings()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1/apicore;");
            return storageAccount.CreateCloudBlobClient();
        }
    }
}
