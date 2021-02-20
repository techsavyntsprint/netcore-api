using APICore.API.Controllers;
using APICore.Services;
using APICore.Services.Impls;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using Wangkanai.Detection.Services;

namespace APICore.Test.Mocks
{
    class RegisterAction
    {
        public static AccountController RegisterEndpoint
        {
            get
            {
                var accountService = new AccountService(new Mock<IConfiguration>().Object, new DBContext().MockRepository().Object, new Mock<IStringLocalizer<IAccountService>>().Object, new Mock<IDetectionService>().Object, GetBlobHardCodedSettings());
                var accountController = new AccountController(accountService, new Mock<AutoMapper.IMapper>().Object, new Mock<IEmailService>().Object, new Mock<IWebHostEnvironment>().Object);

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
