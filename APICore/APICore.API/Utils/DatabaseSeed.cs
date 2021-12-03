using APICore.Data.Entities;
using APICore.Data.Entities.Enums;
using APICore.Data.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APICore.API.Utils
{
    public static class DatabaseSeed
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            var _uow = serviceProvider.GetRequiredService<IUnitOfWork>();

            await CreateDefaultUserAsync(_uow);
        }

        private static async Task CreateDefaultUserAsync(IUnitOfWork uow)
        {
            var admin = await uow.UserRepository.FindBy(u => u.Email == "admin@ntsprint.com").FirstOrDefaultAsync();

            if (admin == null)
            {
                using (var hashAlgorithm = new SHA256CryptoServiceProvider())
                {
                    var byteValue = Encoding.UTF8.GetBytes("P@sssw0rd");
                    var byteHash = hashAlgorithm.ComputeHash(byteValue);

                    admin = new User()
                    {
                        FullName = "Admin",
                        Email = "admin@ntsprint.com",
                        Password = Convert.ToBase64String(byteHash),
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        Status = StatusEnum.ACTIVE
                    };

                    await uow.UserRepository.AddAsync(admin);
                    await uow.CommitAsync();
                }
            }
        }
    }
}