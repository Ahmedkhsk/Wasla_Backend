namespace Wasla_Backend.Repositories.Implementation
{
    public class DoctorServiceRepository : GenericRepository<Service>, IDoctorServiceRepository 
    {
        public DoctorServiceRepository(Context context) : base(context)
        {
        }
        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _dbSet
                .Include(s => s.ServiceDays)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (entity == null)
                return;

            if (entity.ServiceDays != null && entity.ServiceDays.Any())
                _context.Set<ServiceDay>().RemoveRange(entity.ServiceDays);

            _dbSet.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<Service> GetServiceIncludeDaysAsync(int id)
            => await _context.Service
                .Include(d => d.ServiceDays)
                .FirstOrDefaultAsync(i => i.Id == id && i.IsDeleted == false);
        public async Task<IEnumerable<Service>> GetAllServicesAsync(string id)
            => await _context.Service
                .Include(d => d.ServiceDays)
                .Where(i => i.ServiceProviderId == id && i.IsDeleted == false)
                .ToListAsync();

        public async Task<Service> GetAllServiceAsync(int id)
            => await _context.Service
                .Include(d => d.ServiceDays)
                .FirstOrDefaultAsync(i => i.Id == id && i.IsDeleted == false);

    }
}
