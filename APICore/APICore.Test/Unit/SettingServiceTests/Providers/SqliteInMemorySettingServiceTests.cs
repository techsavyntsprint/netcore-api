using System;
using System.Data.Common;
using APICore.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace APICore.Test.Unit.SettingServiceTests.Providers
{
    public class SqliteInMemorySettingServiceTests : SettingServiceTests, IDisposable
    {
        private readonly DbConnection _connection;

        public SqliteInMemorySettingServiceTests() : base(new DbContextOptionsBuilder<CoreDbContext>()
                                                         .UseSqlite(CreateInMemoryDatabase())
                                                         .Options)
        {
            _connection = RelationalOptionsExtension.Extract(ContextOptions)
                                                    .Connection;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose()
        {
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
