using APICore.Data;
using APICore.Data.Entities;
using APICore.Data.Repository;
using APICore.Data.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICore.Test
{
    class Setup
    {
        //BLOB Setting are needed and for now they are hardcodded.
        public static CloudBlobClient GetBlobHardCodedSettings()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1/apicore;");
            return storageAccount.CreateCloudBlobClient();
        }
        public static Mock<IUnitOfWork> MockDatabase()
        {
            var uowMock = new Mock<IUnitOfWork>();

            // In-memory database
            var options = new DbContextOptionsBuilder<CoreDbContext>()
                     .UseInMemoryDatabase(databaseName: "MockDB")
                     .Options;

            var fakeDb = new CoreDbContext(options);
            fakeDb.Add(new User
            {
                Email = "carlos@itguy.com",
                FullName = "Carlos Delgado",
                Gender = 0,
                Phone = "+53 12345678",
                Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0="
            });
            fakeDb.SaveChanges();

            var fakeUserRepo = new GenericRepository<User>(fakeDb);
            var fakeUserTokenRepo = new GenericRepository<UserToken>(fakeDb);
            uowMock.Setup(repo => repo.UserRepository).Returns(fakeUserRepo);
            uowMock.Setup(repo => repo.UserTokenRepository).Returns(fakeUserTokenRepo);
            return uowMock;
        }
    }
}
