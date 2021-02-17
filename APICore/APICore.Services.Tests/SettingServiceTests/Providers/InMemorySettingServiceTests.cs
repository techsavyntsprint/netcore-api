using APICore.Data;
using Microsoft.EntityFrameworkCore;

namespace APICore.Services.Tests.SettingServiceTests.Providers
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
