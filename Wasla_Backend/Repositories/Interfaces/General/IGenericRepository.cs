using System.Linq.Expressions;

namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<T> GetByIdAsync(string id);
        public Task AddAsync(T entity);
        public Task AddRangeAsync(IEnumerable<T> entities);
        public void RemoveRange(IEnumerable<T> entities);
        public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        public void Update(T entity);
        public void Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        public Task SaveChangesAsync();
    }
}
