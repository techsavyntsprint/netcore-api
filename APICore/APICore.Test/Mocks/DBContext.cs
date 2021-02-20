using APICore.Data;
using APICore.Data.Entities;
using APICore.Data.Entities.Enums;
using APICore.Data.Repository;
using APICore.Data.UoW;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;

namespace APICore.Test
{
    class DBContext
    {
        private readonly Mock<IUnitOfWork> uowMock;
        private readonly DbContextOptions<CoreDbContext> dbContextOptions;
        private readonly CoreDbContext coreDbContext;
        private const string dbName = "mockedDB";

        public DBContext()
        {
            uowMock = new Mock<IUnitOfWork>();

            dbContextOptions = new DbContextOptionsBuilder<CoreDbContext>()
                     .UseInMemoryDatabase(databaseName: dbName)
                     .Options;

            coreDbContext = new CoreDbContext(dbContextOptions);

            Seed();
        }
        private void Seed()
        {
            if(!coreDbContext.Users.Any())
            {
                // Active User
                coreDbContext.Add(new User
                {
                    Id = 1,
                    Email = "carlos@itguy.com",
                    FullName = "Carlos Delgado",
                    Gender = 0,
                    Phone = "+53 12345678",
                    Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0=",
                    Status = StatusEnum.ACTIVE
                });
                // Inactive User
                coreDbContext.Add(new User
                {
                    Id = 2,
                    Email = "inactive@itguy.com",
                    FullName = "Manuel Delgado",
                    Gender = 0,
                    Phone = "+53 12345678",
                    Password = @"gM3vIavHvte3fimrk2uVIIoAB//f2TmRuTy4IWwNWp0=",
                    Status = StatusEnum.INACTIVE
                });
                // fake Token for Active User`
                coreDbContext.Add(new UserToken
                {
                    Id = 1,
                    AccessToken = "s0m34cc$$3$T0k3n",
                    UserId = 1
                });

                coreDbContext.SaveChanges();
            }
        }
        public Mock<IUnitOfWork> MockRepository()
        {
            var fakeUserRepo = new GenericRepository<User>(coreDbContext);
            var fakeUserTokenRepo = new GenericRepository<UserToken>(coreDbContext);
            uowMock.Setup(repo => repo.UserRepository).Returns(fakeUserRepo);
            uowMock.Setup(repo => repo.UserTokenRepository).Returns(fakeUserTokenRepo);
            return uowMock;
        }
    }
}
