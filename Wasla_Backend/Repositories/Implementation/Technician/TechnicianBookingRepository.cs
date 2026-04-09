

namespace Wasla_Backend.Repositories.Implementation.technician
{
    public class TechnicianBookingRepository : GenericRepository<TechnicianBooking>, ITechnicianBookingRepository
    {
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        public TechnicianBookingRepository(Context context,IFileUrlBuilderService fileUrlBuilderService) : base(context)
        {
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<int> ChangeBookingStatus(int bookingId, TechnicianBookingStatus status)
        {
          return  await _context.TechnicianBookings.Where(tb => tb.Id == bookingId).ExecuteUpdateAsync(setters =>
                setters.SetProperty(tb => tb.Status, status));

        }

        public async Task<BookingDetailsForTechnicianDto> DetailsForTechnician(int bookingId)
        {
            return await _context.TechnicianBookings.Where(tb => tb.Id == bookingId)
                .Select(tb => new BookingDetailsForTechnicianDto
                {
                    BookingId = tb.Id,
                    ResidentName = tb.Resident.FullName,
                    ResidentPhone = tb.Resident.Phone,
                    ResidentImage = tb.Resident.ProfilePhoto,
                    Latitude = tb.Resident.Latitude,
                    Longitude = tb.Resident.Longitude,
                    price = tb.Price,
                    BookingDate = tb.BookingDate,
                    Status = tb.Status
                }).FirstOrDefaultAsync();
        }

        public async Task<List<TechnicianBookingOfResident>> GetByResidentIdAndSpecialization(string residentId, TechnicianSpecialty specialization)
        {
            return await _context.TechnicianBookings.Where(tb => tb.ResidentId == residentId&&tb.Specialty==specialization)
              .Select(tb => new TechnicianBookingOfResident
              {
                  BookingId = tb.Id,
                  TechnicianName = tb.Technician.FullName,
                  TechnicianPhone = tb.Technician.Phone,
                  TechnicianImage = tb.Technician.ProfilePhoto,
                  price = tb.Price,
                  BookingDate = tb.BookingDate,
                  Status = tb.Status
              }).ToListAsync();
        }

        public async Task<bool> IsExist(int bookingId)
        {
            return await _context.TechnicianBookings.AnyAsync(tb => tb.Id == bookingId);
        }

        public async Task<List<TechnicianBookingOfResident>> technicianBookingOfResidents(string residentId)
        {
            return await _context.TechnicianBookings.Where(tb=>tb.ResidentId==residentId)
                .Select(tb => new TechnicianBookingOfResident
            {
                BookingId = tb.Id,
                TechnicianName = tb.Technician.FullName,
                TechnicianPhone = tb.Technician.Phone,
                TechnicianImage = tb.Technician.ProfilePhoto,
                price = tb.Price,
                BookingDate = tb.BookingDate,
                Status = tb.Status,
                TechnicianSpeciality = tb.Specialty
                }).ToListAsync();
        }

        public async Task<List<BookingDetailsForTechnicianDto>> technicianBookingOfTechnician(string technicianId)
        {
            return await _context.TechnicianBookings.Where(tb => tb.TechnicianId == technicianId)
                .Select(tb => new BookingDetailsForTechnicianDto
                {
                    BookingId = tb.Id,
                    ResidentName = tb.Resident.FullName,
                    ResidentPhone = tb.Resident.Phone,
                    ResidentImage = tb.Resident.ProfilePhoto,
                    Latitude = tb.Resident.Latitude,
                    Longitude = tb.Resident.Longitude,
                    price = tb.Price,
                    BookingDate = tb.BookingDate,
                    Status = tb.Status
                }).ToListAsync();
        }
    }
}
