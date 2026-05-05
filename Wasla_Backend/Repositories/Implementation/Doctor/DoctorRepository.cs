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
                .Where(d => d.Status == UserStatus.Active)
                .OrderByDescending(d => d.Rating)
                .ToListAsync();
        }

        public async Task<Doctor> GetByEmail(string email)
        {
            return await _userManager.Users
                .Where(d => d.Status == UserStatus.Active)
                .OfType<Doctor>()
                .FirstOrDefaultAsync(d => d.Email == email);
        }
        public async Task<DoctorProfileResponse> GetDoctorProfileById(string id)
        {
          return await _context.Doctors.Where(d=>d.Id==id && d.Status == UserStatus.Active).AsNoTracking().Select(d=> new DoctorProfileResponse
          {
             email= d.Email,
              fullName = d.FullName,
              specializationName = d.Specialization!.Specialization.GetText("en"),
              experienceYears = d.ExperienceYears,
              universityName = d.UniversityName,
              hospitalname = d.hospitalname,
              numberOfpatients = d.numberOfpatients,
              BookingCount = _context.Booking
                .Count(b => b.serviceProviderId == d.Id
                         && b.bookingStatus == BookingStatus.completed),
              ReviewsCount = d.Reviews.Count(),
              graduationYear = d.GraduationYear,
              birthDay = d.BirthDay,
              phone = d.Phone,
              latitude = d.Latitude,
              longitude = d.Longitude,
              description = d.Description,
              image = d.ProfilePhoto,
              cv = d.CV,
              rating = d.Rating

          }).FirstOrDefaultAsync();  
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialist(int specialistId)
        {
            return await _context.Doctors
                .AsNoTracking()
                .Where(d => d.SpecializationId == specialistId && d.Status == UserStatus.Active)
                .OrderByDescending(d=>d.Rating)
                .ToListAsync();
        }

        public async Task<AllDoctorDataDto?> GetDoctorData(string doctorId)
        {
         var doctor = await _context.Doctors
         .AsNoTracking()
         .Where(d => d.Id == doctorId && d.Status == UserStatus.Active)
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
                .Where(d => d.Id == doctorId && d.Status == UserStatus.Active)
                .Select(d => d.Specialization!.Specialization)
                .FirstOrDefaultAsync();

            return specialization?.GetText(language);
        }


    }
}
