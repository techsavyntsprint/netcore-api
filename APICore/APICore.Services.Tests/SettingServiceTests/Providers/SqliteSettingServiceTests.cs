using APICore.Data;
using Microsoft.EntityFrameworkCore;

namespace APICore.Services.Tests.SettingServiceTests.Providers
{
    public class SqliteSettingServiceTests : SettingServiceTests
    {
        public SqliteSettingServiceTests() : base(new DbContextOptionsBuilder<CoreDbContext>()
                                                 .UseSqlite("Filename=Test.db")
                                                 .Options)
        {
        }
    }
}
