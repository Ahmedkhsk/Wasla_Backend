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
                .Include(s => s.ServiceDates)
                .Include(s => s.TimeSlots)
                .FirstOrDefaultAsync(s => s.id == id);

            if (entity == null)
                return;

            if (entity.ServiceDays != null && entity.ServiceDays.Any())
                _context.Set<ServiceDay>().RemoveRange(entity.ServiceDays);

            if (entity.ServiceDates != null && entity.ServiceDates.Any())
                _context.Set<ServiceDate>().RemoveRange(entity.ServiceDates);

            if (entity.TimeSlots != null && entity.TimeSlots.Any())
                _context.Set<TimeSlot>().RemoveRange(entity.TimeSlots);

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        
        
        public async Task<IEnumerable<Service>> GetAllServicesAsync(string id)
            => await _context.Service
                .Include(d => d.ServiceDays)
                .Include(s => s.ServiceDates)
                .Include(t => t.TimeSlots)
                .Where(i => i.doctorId == id)
                .ToListAsync();

        public async Task<Service> GetAllServiceAsync(int id)
            => await _context.Service
                .Include(d => d.ServiceDays)
                .Include(s => s.ServiceDates)
                .Include(t => t.TimeSlots)
                .FirstOrDefaultAsync(i => i.id == id);

    }
}
