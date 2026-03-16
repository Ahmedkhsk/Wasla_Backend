


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
    }
}
