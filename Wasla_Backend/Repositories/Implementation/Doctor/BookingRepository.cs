
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
                   && b.ServiceProviderType == ServiceProviderType.Doctor)
                .CountAsync();
        }
        public async Task<int> CountBookings(BookingStatus status)
        {
            return await _context.Booking
                .Where(b => b.bookingStatus == status)
                .CountAsync();
        }
        public async Task<List<GetAllBookingResponse>> GetBookingsByDoctorIdAsync(
           string doctorId, BookingStatus status, string lan)
        {
            var query = _context.Booking
                .Where(b => b.serviceProviderId == doctorId
                    && b.ServiceProviderType == ServiceProviderType.Doctor)
                .OrderByDescending(b => b.Date)
                .Include(b => b.serviceDay)
                    .ThenInclude(sd => sd.service)
                .Include(b => b.Resident)
                .AsNoTracking();

            if (status != BookingStatus.all)
            {
                query = query.Where(b => b.bookingStatus == status);
            }

            return await query
                .Select(b => new GetAllBookingResponse
                {
                    bookingId = b.Id,
                    date = b.Date,
                    start = b.newStart ?? b.serviceDay.start,
                    end = b.newEnd ?? b.serviceDay.end,
                    day = !(b.newDayOfWeek == WeekDayEnum.none)
                            ? b.newDayOfWeek
                            : b.serviceDay.dayOfWeek,
                    status = b.bookingStatus,
                    serviceName = lan.ToLower() == "ar"
                        ? b.serviceDay.service.serviceName.Arabic
                        : b.serviceDay.service.serviceName.English,
                    userName = b.Resident.FullName,
                    userImage = b.Resident.ProfilePhoto,
                    bookingType = b.bookingType,
                    phone = b.Resident.Phone,
                    price = (decimal)b.price,
                    bookingImages = b.images,
                    isPaid=b.IsPaid
                })
                .ToListAsync();
        }

        public async Task<int> CountCompletedBookings(string doctorId)
        {
            return await _context.Booking
                .Where(b => b.serviceProviderId == doctorId
                   && b.ServiceProviderType == ServiceProviderType.Doctor
                   && b.bookingStatus == BookingStatus.completed)
                .CountAsync();
        }

        public async Task<int> CountPatients(string doctorId)
        {
            return await _context.Booking
                  .Where(b => b.serviceProviderId == doctorId
                   && b.ServiceProviderType == ServiceProviderType.Doctor)
                  .Select(b => b.ResidentId)
                  .Distinct()
                  .CountAsync();
        }

        public async Task<Booking> GetByIdWithIncludeAsync(int id)
        {
            return await _context.Booking
                .Include(b => b.Resident)
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
                .Where(b => b.ResidentId == userId)
                .OrderByDescending(b => b.Date)

                .Include(b => b.serviceDay)
                    .ThenInclude(sd => sd.service)
                        .ThenInclude(s => s.ServiceProvider)
                .Select(b => new ServiceBookingDetailsDto
                {
                    id = b.Id,
                    start = b.newStart ?? b.serviceDay.start,
                    end = b.newEnd ?? b.serviceDay.end,
                    day = b.newDayOfWeek != WeekDayEnum.none ? b.newDayOfWeek : b.serviceDay.dayOfWeek,
                    date = b.Date,
                    status = b.bookingStatus,
                    ServiceProviderName = b.serviceDay.service.ServiceProvider.FullName,
                    ServiceProviderProfilePhoto = b.serviceDay.service.ServiceProvider.ProfilePhoto,
                    ServiceName = language.ToLower() == "ar"
                    ? b.serviceDay.service.serviceName.Arabic
                    : b.serviceDay.service.serviceName.English,
                    Price = b.price,
                    isPaid = b.IsPaid
                }).ToListAsync();

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
                .AnyAsync(b => b.ResidentId == userId && b.serviceProviderId == doctorId);
        }

        public async Task<List<CollectedPerYearDto>> GetCollectedPriceByYear(string doctorId)
        {
            return await _context.Booking
                .Where(b => b.serviceProviderId == doctorId
                    && b.ServiceProviderType == ServiceProviderType.Doctor && b.bookingStatus == BookingStatus.completed)
                .GroupBy(b => b.Date.Year)
                .Select(yearGroup => new CollectedPerYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(b => b.Date.Month)
                        .Select(monthGroup => new CollectedPerMonthDto
                        {
                            month = monthGroup.Key,
                            amount = monthGroup.Sum(b => b.price)
                        })
                        .OrderBy(m => m.month)
                        .ToList()
                })
                .OrderBy(y => y.year)
                .ToListAsync();
        }

        public async Task<List<CollectedPerYearDto>> GetCollectedPriceBookingsPerYear()
        {
            return await _context.Booking
                .Where(b => b.bookingStatus == BookingStatus.completed)
                .GroupBy(b => b.Date.Year)
                .Select(yearGroup => new CollectedPerYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(b => b.Date.Month)
                        .Select(monthGroup => new CollectedPerMonthDto
                        {
                            month = monthGroup.Key,
                            amount = monthGroup.Sum(b => b.price)
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
                   && b.ServiceProviderType == ServiceProviderType.Doctor)
                  .Select(b => b.ResidentId)
                  .Distinct()
                  .CountAsync();
        }

        public async Task<decimal> GetTotalAmount(string doctorId)
        {
            return await _context.Booking
                .Where(b=>b.serviceProviderId == doctorId && 
                       b.ServiceProviderType == ServiceProviderType.Doctor &&
                       b.bookingStatus == BookingStatus.completed)
                .SumAsync(b => (decimal)b.price);
        }
        public async Task<List<Booking>> GetBookingsForResidentAsync(string residentId)
        {
            return await _context.Booking
                .Where(b => b.ResidentId == residentId)
                .ToListAsync();
        }

        public Task<int> CountBookingBYUserAndServiceProvider(string userId, string serviceProviderId)
        {
            return _context.Booking
                .Where(b => b.ResidentId == userId && b.serviceProviderId == serviceProviderId)
                .CountAsync();
        }

        public async Task<bool> HasBookingSameDay(string userId, string ServiceProviderId, DateTime date)
        {
            return await _context.Booking.AnyAsync(b => b.ResidentId == userId && b.Date == date
            && b.bookingStatus != BookingStatus.canceled && b.serviceProviderId == ServiceProviderId);
        }

        public async Task<Booking> GetWithService(int bookingId)
        {
            return await _context.Booking
                .Include(b => b.serviceDay)
                    .ThenInclude(sd => sd.service)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }
    }
}
