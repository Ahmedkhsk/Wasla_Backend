namespace Wasla_Backend.Repositories.Implementation.Gyms
{
    public class GymBookingRepository: GenericRepository<GymBooking>, IGymBookingRepository
    {
        public GymBookingRepository(Context context) : base(context)
        {
        }

        public async Task<List<BookingOfGym>> PackagebookingOfGym(string gymId)
        {
            return await _context.GymBooking.AsNoTracking().Where(b => b.GymId == gymId).Include(b=>b.Resident).Include(b=>b.Service)
                .Select(b => new BookingOfGym
                {
                  bookingId=b.Id,
                  name=b.Resident.FullName,
                  imageUrl=b.Resident.ProfilePhoto,
                  bookingTime = b.BookingDate,
                  price = b.price,
                  serviceName=b.Service.Name,
                  DurationInMonths= b.Service.DurationInMonths,
                  bookingStatus=b.BookingStatus
                }).ToListAsync();
                ;
        }

        public async Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status)
        {
            return await _context.GymBooking.AsNoTracking().Where(b => b.GymId == gymId && b.BookingStatus==status)
                .Include(b => b.Resident)
                .Include(b => b.Service)
                .Select(b => new BookingOfGym
                {
                    bookingId = b.Id,
                    name = b.Resident.FullName,
                    imageUrl = b.Resident.ProfilePhoto,
                    bookingTime = b.BookingDate,
                    serviceName = b.Service.Name,
                    price = b.price,
                    DurationInMonths = b.Service.DurationInMonths,
                    bookingStatus = b.BookingStatus

                }).ToListAsync();
            
        }

        public async Task<List<BookingOfUser>> PackagebookingOfResident(string residentId)
        {
            return await _context.GymBooking
     .AsNoTracking()
     .Where(b => b.ResidentId == residentId)
     .Include(b => b.Gym)
     .Include(b => b.Service)
     .Select(b => new BookingOfUser
     {
         bookingId = b.Id,
         GymName = b.Gym.BusinessName,
         imageUrl = b.Gym.ProfilePhoto,
         bookingTime = b.BookingDate,
         serviceName = b.Service.Name,
         DurationInMonths = b.Service.DurationInMonths,
         bookingStatus = b.BookingStatus,
         IsPaid = b.IsPaid
     })
     .ToListAsync();


        }

        public async Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status)
        {
            return await _context.GymBooking.AsNoTracking().Where(b => b.ResidentId == residentId && b.BookingStatus == status).Include(b => b.Gym)
                .Include(b => b.Service)
                .Select(b => new BookingOfUser
                {
                    bookingId = b.Id,
                    GymName = b.Gym.FullName,
                    imageUrl = b.Gym.ProfilePhoto,
                    bookingTime = b.BookingDate,
                    serviceName = b.Service.Name,
                    DurationInMonths = b.Service.DurationInMonths,
                    bookingStatus = b.BookingStatus,
                    IsPaid = b.IsPaid
                }).ToListAsync();
        }

        public async Task<List<UserPackageResponse>> UserPackageResponses(int serviceId)
        {
            return await _context.GymBooking
                .Where(b => b.Service.Id == serviceId && b.BookingStatus != GymBookingStatus.Cancelled)
                .Select(b => b.Resident)
                .Distinct()
                .Select(r => new UserPackageResponse
                {
                    name = r.FullName,
                    email = r.Email,
                    phone = r.Phone,
                    image = r.ProfilePhoto
                })
                .ToListAsync();
        }

        public async Task<int> GetNumOfTrainee(string id)
            => await _context.GymBooking.Where(i => i.GymId == id)
                .Select(u => u.ResidentId)
                .Distinct()
                .CountAsync();
        
        public async Task<decimal> GetTotalAmount(string id)
            => await _context.GymBooking.Where(i => i.GymId == id)
                .SumAsync(d => d.price);

        public async Task<int> GetNumberOfBookings(string id)
            =>  await _context.GymBooking.CountAsync(i => i.GymId == id);

        public async Task<List<CollectedPerYearDto>> GetCollectedPriceByYear(string id)
            => await _context.GymBooking.Where(i => i.GymId == id)
                .GroupBy(b => b.BookingDate.Year)
                .Select
                (
                    yearGroub => new CollectedPerYearDto
                    {
                        year = yearGroub.Key,
                        months = yearGroub.GroupBy(b => b.BookingDate.Month)
                                 .Select(
                                    monthGroub => new CollectedPerMonthDto
                                    {
                                        month = monthGroub.Key,
                                        amount = monthGroub.Sum(d => d.price)
                                    }).OrderBy(m => m.month).ToList()
                    }
                ).OrderBy(y => y.year).ToListAsync();

        public Task<bool> IsBookingExist(string residentId, int serviceId)
        {
            return _context.GymBooking.
                AnyAsync(b => b.ResidentId == residentId && b.ServiceId == serviceId && b.BookingStatus == GymBookingStatus.Active);
        }
    }
}
