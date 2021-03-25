using APICore.Data;
using Microsoft.EntityFrameworkCore;

namespace APICore.Test.SettingServiceTests.Providers
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
