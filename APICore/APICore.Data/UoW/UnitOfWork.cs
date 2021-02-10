using APICore.Data.Entities;
using APICore.Data.Repository;
using System;
using System.Threading.Tasks;

namespace APICore.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoreDbContext _context;

        private bool disposed = false;

        public UnitOfWork(CoreDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            UserRepository = UserRepository ?? new GenericRepository<User>(_context);
            UserTokenRepository = UserTokenRepository ?? new GenericRepository<UserToken>(_context);
            SettingRepository = SettingRepository ?? new GenericRepository<Setting>(_context);
            LogRepository = LogRepository ?? new GenericRepository<Log>(_context);
        }

        public IGenericRepository<User> UserRepository { get; set; }
        public IGenericRepository<UserToken> UserTokenRepository { get; set; }
        public IGenericRepository<Setting> SettingRepository { get; set; }
        public IGenericRepository<Log> LogRepository { get; set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}