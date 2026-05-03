


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
           return await _context.Technicians.Where(t => t.Id == id).AsNoTracking()
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
                    NumberOfReviews = t.Reviews.Count(),
                    BookingCount = _context.TechnicianBookings.Count(tb => tb.TechnicianId == t.Id&&tb.Status==TechnicianBookingStatus.Done),
                    NumberOfResident = _context.TechnicianBookings.Where(tb => tb.TechnicianId == t.Id && tb.Status == TechnicianBookingStatus.Done).Select(tb => tb.ResidentId).Distinct().Count(),
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

        public async Task<TechnicianChartDto> GetChartById(string TechnicianId)
        {
           
            var bookingquery = _context.TechnicianBookings
                .AsNoTracking()
                .Where(r => r.TechnicianId == TechnicianId && r.Status == TechnicianBookingStatus.Done);

            var numberOfRides = await bookingquery.CountAsync();

            var numberOfDeliveredResident = await bookingquery
                .Select(r => r.ResidentId)
                .Distinct()
                .CountAsync();

            var totalAmount = await bookingquery.SumAsync(r => r.price);

            var years = await bookingquery
                .GroupBy(r => r.Date.Year)
                .Select(yearGroup => new CollectedPerYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(r => r.Date.Month)
                        .Select(monthGroup => new CollectedPerMonthDto
                        {
                            month = monthGroup.Key,
                            amount = monthGroup.Sum(r => r.price)
                        })
                        .OrderBy(m => m.month)
                        .ToList()
                })
                .OrderBy(y => y.year)
                .ToListAsync();

            return new TechnicianChartDto
            {
                CompletedBookings = numberOfRides,
                NumberOfResidents = numberOfDeliveredResident,
                totalAmount = totalAmount,
                years = years
            };
        }
    }
    
}
