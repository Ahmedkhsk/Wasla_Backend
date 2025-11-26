using System.Linq.Expressions;

namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<T> GetByIdAsync(string id);
        public Task AddAsync(T entity);
        public void RemoveRange(IEnumerable<T> entities);
        public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        public void Update(T entity);
        public void Delete(T entity);
        public Task SaveChangesAsync();


    }
}
