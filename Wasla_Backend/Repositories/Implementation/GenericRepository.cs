namespace Wasla_Backend.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly Context _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(Context context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null)
                return;
            
            _dbSet.RemoveRange(entities);
        }
           

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task<T> GetByIdAsync(string id)
            => await _dbSet.FindAsync(id);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
