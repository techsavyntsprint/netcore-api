using APICore.Data;
using Microsoft.EntityFrameworkCore;

namespace APICore.Test.Unit.SettingServiceTests.Providers
{
    public class InMemorySettingServiceTests : SettingServiceTests
    {
        public InMemorySettingServiceTests() : base(new DbContextOptionsBuilder<CoreDbContext>()
                                                   .UseInMemoryDatabase("TestDatabase")
                                                   .Options)
        {
        }
    }
}
