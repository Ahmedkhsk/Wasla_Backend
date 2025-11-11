

namespace Wasla_Backend.Repositories.Implementation
{
    public class ResidentRepository : GenericRepository<Resident>, IResidentRepository
    {
        public ResidentRepository(Context context) : base(context)
        {
        }

        public async Task<Resident> GetByEmail(string email)
        {
           return await _context.Residents.FirstOrDefaultAsync(r => r.Email == email);
        }
    }
}
