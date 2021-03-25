using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APICore.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T Find(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        T Get(int id);

        IQueryable<T> GetAll();

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T t);

        Task AddRangeAsync(List<T> t);

        Task<int> CountAsync();

        void Delete(T entity);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> GetAllAsync();

        Task<T> GetAsync(int id);

        Task UpdateAsync(T t, object key);

        void Update(T t);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}