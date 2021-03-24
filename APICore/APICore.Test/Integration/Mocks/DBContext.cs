using APICore.Data;
using APICore.Data.Entities;
using APICore.Data.Entities.Enums;
using APICore.Data.Repository;
using APICore.Data.UoW;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace APICore.Tests.Integration.Mocks
{
    class FakeDbContext
    {
        private Mock<IUnitOfWork> uowMock;
        private DbContextOptions<CoreDbContext> dbContextOptions;
        private CoreDbContext coreDbContext;
        private const string dbName = "mockedDB";

        public Mock<IUnitOfWork> MockRepository()
        {
            uowMock = new Mock<IUnitOfWork>();

            dbContextOptions = new DbContextOptionsBuilder<CoreDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            coreDbContext = new CoreDbContext(dbContextOptions);

            if (!coreDbContext.Users.Any())
            {
                coreDbContext.Users.AddRangeAsync(new User
                {
                    Id = 1,
                    Email = "carlos@itguy.com",
                    FullName = "Carlos Delgado",
                    Gender = 0,
                    Phone = "+53 12345678",
                    Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0=",
                    Status = StatusEnum.ACTIVE
                }, new User
                {
                    Id = 2,
                    Email = "inactive@itguy.com",
                    FullName = "Manuel Delgado",
                    Gender = 0,
                    Phone = "+53 12345678",
                    Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0=",
                    Status = StatusEnum.INACTIVE,
                    Identity = "someRandomIdentityString"
                });

                coreDbContext.SaveChanges();
            }

            var fakeUserRepo = new GenericRepository<User>(coreDbContext);
            var fakeUserTokenRepo = new GenericRepository<UserToken>(coreDbContext);
            uowMock.Setup(repo => repo.UserRepository).Returns(fakeUserRepo);
            uowMock.Setup(repo => repo.UserTokenRepository).Returns(fakeUserTokenRepo);

            return uowMock;
        }
    }
}
