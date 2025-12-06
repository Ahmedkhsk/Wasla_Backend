
namespace Wasla_Backend.Repositories.Implementation
{
    public class BookingRepository:GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(Context context) : base(context)
        {
        }

        public async Task<int> CountBookings(string doctorId)
        {
            return await _context.Booking
                .Where(b => b.serviceProviderId == doctorId
                   && b.serviceProviderType == ServiceProviderType.Doctor)
                .CountAsync();
        }

        public async Task<List<GetAllBookingResponse>> GetBookingsByDoctorIdAsync(
           string doctorId, BookingStatus status, string lan)
        {
            var query = _context.Booking
                .Where(b => b.serviceProviderId == doctorId
                    && b.serviceProviderType == ServiceProviderType.Doctor)
                .Include(b => b.serviceDay)
                    .ThenInclude(sd => sd.service)
                .Include(b => b.user)
                .AsNoTracking();

            if (status != BookingStatus.all)
            {
                query = query.Where(b => b.bookingStatus == status);
            }

            return await query
                .Select(b => new GetAllBookingResponse
                {
                    bookingId = b.Id,
                    date = b.bookingDate.ToString(),
                    start = !string.IsNullOrWhiteSpace(b.newStart)
                            ? b.newStart
                            : b.serviceDay.start,
                    end = !string.IsNullOrWhiteSpace(b.newEnd)
                            ? b.newEnd
                            : b.serviceDay.end,
                    day = !(b.newDayOfWeek == WeekDayEnum.none)
                            ? b.newDayOfWeek
                            : b.serviceDay.dayOfWeek,
                    status = b.bookingStatus,
                    serviceName = lan.ToLower() == "ar"
                        ? b.serviceDay.service.serviceName.Arabic
                        : b.serviceDay.service.serviceName.English,
                    userName = b.user.FullName,
                    userImage = b.user.ProfilePhoto,
                    bookingType = b.bookingType,
                    phone = b.user.Phone,
                    price = (decimal)b.price,
                    bookingImages = b.images
                })
                .ToListAsync();
        }

        public async Task<int> CountCompletedBookings(string doctorId)
        {
            return await _context.Booking
                .Where(b => b.serviceProviderId == doctorId
                   && b.serviceProviderType == ServiceProviderType.Doctor
                   && b.bookingStatus == BookingStatus.completed)
                .CountAsync();
        }

        public async Task<int> CountPatients(string doctorId)
        {
            return await _context.Booking
                  .Where(b => b.serviceProviderId == doctorId
                   && b.serviceProviderType == ServiceProviderType.Doctor)
                  .Select(b => b.userId)
                  .Distinct()
                  .CountAsync();
        }

        public async Task<Booking> GetByIdAsyncWithInclude(int id)
        {
            return await _context.Booking
                .Include(b => b.user)
                .Include(b => b.serviceDay)
                    .ThenInclude(sd => sd.service)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Booking> GetBookingByServiceDayIdAsync(int serviceDayId)
        {
            return await _context.Booking
                .FirstOrDefaultAsync(b => b.serviceDayId == serviceDayId);
        }

        public async Task<List<ServiceBookingDetailsDto>> GetBookingDetailsForUserAsync(string userId, string language)
        {
            var bookingDetails = await _context.Booking
                .Where(b => b.userId == userId)
                .Include(b => b.serviceDay)
                    .ThenInclude(sd => sd.service)
                        .ThenInclude(s => s.doctor)
                .Select(b => new ServiceBookingDetailsDto
                {
                    id = b.Id,
                    start = !string.IsNullOrWhiteSpace(b.newStart)
                            ? b.newStart
                            : b.serviceDay.start,
                    end = !string.IsNullOrWhiteSpace(b.newEnd)
                            ? b.newEnd
                            : b.serviceDay.end,
                    day = !(b.newDayOfWeek == WeekDayEnum.none)
                            ? b.newDayOfWeek
                            : b.serviceDay.dayOfWeek,
                    date = b.bookingDate.ToString(),
                    status = b.bookingStatus,

                    ServiceProviderName = b.serviceDay.service.doctor.FullName,
                    ServiceProviderProfilePhoto = b.serviceDay.service.doctor.ProfilePhoto,

                    ServiceName = language.ToLower() == "ar"
                        ? b.serviceDay.service.serviceName.Arabic
                        : b.serviceDay.service.serviceName.English,

                    Price = b.price
                })
                .ToListAsync();

            return bookingDetails;
        }

        public async Task<Booking> GetByServiceDayId(int serviceDayId)
        {
            return await _context.Booking
                .FirstOrDefaultAsync(b => b.serviceDayId == serviceDayId);
        }

        public async Task<List<Booking>> GetByServiceProviderId(string userId)
        {
            return await _context.Booking
                .Where(b => b.serviceProviderId == userId)
                .ToListAsync();
        }
        

        public async Task<bool> GetByUserIdAndDoctorID(string userId, string doctorId)
        {
           return await _context.Booking
                .AnyAsync(b => b.userId == userId && b.serviceProviderId == doctorId);
        }

        public async Task<List<CollectedPricePerYearDto>> GetCollectedPriceByYear(string doctorId)
        {
            return await _context.Booking
                .Where(b => b.serviceProviderId == doctorId
                    && b.serviceProviderType == ServiceProviderType.Doctor && b.bookingStatus == BookingStatus.completed)
                .GroupBy(b => b.bookingDate.Year)
                .Select(yearGroup => new CollectedPricePerYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(b => b.bookingDate.Month)
                        .Select(monthGroup => new CollectedPricePerMonthDto
                        {
                            month = monthGroup.Key,
                            amount = monthGroup.Sum(b => (decimal)b.price)
                        })
                        .OrderBy(m => m.month)
                        .ToList()
                })
                .OrderBy(y => y.year)
                .ToListAsync();
        }

        public async Task<int> GetNumberOfPatientByDoctorId(string doctorId)
        {
            return await _context.Booking
                    .Where(b => b.serviceProviderId == doctorId
                   && b.serviceProviderType == ServiceProviderType.Doctor)
                  .Select(b => b.userId)
                  .Distinct()
                  .CountAsync();
        }

        public async Task<decimal> GetTotalAmount(string doctorId)
        {
            return await _context.Booking
                .Where(b=>b.serviceProviderId == doctorId && 
                       b.serviceProviderType == ServiceProviderType.Doctor &&
                       b.bookingStatus == BookingStatus.completed)
                .SumAsync(b => (decimal)b.price);
        }
        public async Task<List<Booking>> GetBookingsForResidentAsync(string residentId)
        {
            return await _context.Booking
                .Where(b => b.userId == residentId)
                .ToListAsync();
        }

    }
}
