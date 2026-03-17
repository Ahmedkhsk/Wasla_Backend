


using Wasla_Backend.Helpers.Extensions;

namespace Wasla_Backend.Repositories.Implementation.technician
{
    public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
    {
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        public TechnicianRepository(Context context,IFileUrlBuilderService fileUrlBuilderService) : base(context)
        {
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<Technician> GetByEmailAsync(string email)
        {
            return await _context.Technicians.FirstOrDefaultAsync(t => t.Email == email);
        }

        public async Task<TechnicianProfileDto> GetProfileById(string id)
        {
           return await _context.Technicians.Where(t => t.Id == id)
                .Select(t => new TechnicianProfileDto
                {
                    Email = t.Email,
                    FullName = t.FullName,
                    Phone = t.Phone,
                    IsAvailable = t.IsAvailable,
                    BirthDay=t.BirthDay,
                    ExperienceYears = t.ExperienceYears,
                    Description = t.Description,
                    Latitude = t.Latitude,
                    Longitude = t.Longitude,

                    Specialty = t.Specialty,
                    Rate = t.Rating,
                    ProfilePhotoUrl=t.ProfilePhoto,
                    DocumentsUrls=t.Documents

                }).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistById(string id)
        {
            return await _context.Technicians.AnyAsync(t => t.Id == id);
        }
        public async Task<List<TechnicianListDto>> GetTechniciansBySpecialty(
    TechnicianSpecialty? specialty,
    int pageNumber,
    int pageSize,
    string lan)
        {
            var query = _context.Technicians
                .AsNoTracking()
                .AsQueryable();

            if (specialty.HasValue)
            {
                query = query.Where(t => t.Specialty == specialty.Value);
            }

            query = query
                .OrderByDescending(t => t.Rating);

            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var technicians = await query
                .Select(t => new TechnicianListDto
                {
                    Id = t.Id.ToString(),
                    Name = t.FullName,
                    Description = t.Description,
                    ImageUrl = t.ProfilePhoto,
                    PhoneNumber = t.Phone,
                    Rating = t.Rating,
                    Specialization = t.Specialty.GetName(lan),
                    YearsOfExperience = (int)t.ExperienceYears
                })
                .ToListAsync();

            return technicians;
        }
    }
}
