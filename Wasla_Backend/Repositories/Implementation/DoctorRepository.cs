using Microsoft.AspNetCore.Identity;

namespace Wasla_Backend.Repositories.Implementation
{
    public class DoctorRepository : GenericRepository<Doctor> , IDoctorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorRepository(UserManager<ApplicationUser> userManager, Context context) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<Doctor> GetByEmail(string email)
        {
            return await _userManager.Users
                .OfType<Doctor>()
                .FirstOrDefaultAsync(d => d.Email == email);
        }
        public async Task<Doctor> GetById(string id)
        {
            return await _userManager.Users
                .OfType<Doctor>()
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialist(int specialistId)
        {
            return await _context.Doctors
                .AsNoTracking()
                .Where(d => d.SpecializationId == specialistId)
                .ToListAsync();
        }

        public async Task<string?> GetDoctorSpecializationName(string doctorId, string language)
        {
            var specialization = await _context.Doctors
                .AsNoTracking()
                .Where(d => d.Id == doctorId)
                .Select(d => d.Specialization!.Specialization)
                .FirstOrDefaultAsync();

            return specialization?.GetText(language);
        }


    }
}
