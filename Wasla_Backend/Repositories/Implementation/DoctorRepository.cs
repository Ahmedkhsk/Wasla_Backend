using Microsoft.AspNetCore.Identity;
using Wasla_Backend.Repositories.Interfaces;

namespace Wasla_Backend.Repositories.Implementation
{
    public class DoctorRepository : GenericRepository<Doctor> , IDoctorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorRepository(UserManager<ApplicationUser> userManager, Context context) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Doctor>> GetAllSortedByRating()
        {
            return await _context.Doctors
                .AsNoTracking()
                .OrderByDescending(d => d.Rating)
                .ToListAsync();
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
                .OrderByDescending(d=>d.Rating)
                .ToListAsync();
        }

        public async Task<AllDoctorDataDto?> GetDoctorData(string doctorId)
        {
         var doctor = await _context.Doctors
         .AsNoTracking()
         .Where(d => d.Id == doctorId)
         .Select(d => new AllDoctorDataDto
         {
             Id = d.Id,
             FullName = d.FullName,
             ExperienceYears = d.ExperienceYears,
             Rating = d.Rating,
             UniversityName = d.UniversityName,
             GraduationYear = d.GraduationYear,
             hospitalname = d.hospitalname,
             numberOfpatients = d.numberOfpatients,
             BirthDay = d.BirthDay,
             Phone = d.Phone,
             Latitude = d.Latitude,
             Longitude = d.Longitude,
             Description = d.Description,
             ImageUrl = d.ProfilePhoto,
             CVUrl = d.CV
         })
         .FirstOrDefaultAsync();

            return doctor;
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
