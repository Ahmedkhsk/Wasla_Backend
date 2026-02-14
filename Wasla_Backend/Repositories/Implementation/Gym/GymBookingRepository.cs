namespace Wasla_Backend.Repositories.Implementation.Gyms
{
    public class GymBookingRepository: GenericRepository<GymBooking>, IGymBookingRepository
    {
        public GymBookingRepository(Context context) : base(context)
        {
        }

        public async Task<List<BookingOfGym>> PackagebookingOfGym(string gymId)
        {
            return await _context.GymBooking.AsNoTracking().Where(b => b.GymId == gymId&& b.Service is Package).Include(b=>b.Resident).Include(b=>b.Service)
                .Select(b => new BookingOfGym
                {
                  bookingId=b.Id,
                  name=b.Resident.FullName,
                  imageUrl=b.Resident.ProfilePhoto,
                  bookingTime = b.BookingDate,
                  price = b.price,
                  serviceName=((Package)b.Service).Name,
                  DurationInMonths=((Package)b.Service).DurationInMonths,
                  bookingStatus=b.BookingStatus
                }).ToListAsync();
                ;
        }

        public async Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status)
        {
            return await _context.GymBooking.AsNoTracking().Where(b => b.GymId == gymId&&b.BookingStatus==status && b.Service is Package)
                .Include(b => b.Resident)
                .Include(b => b.Service)
                .Select(b => new BookingOfGym
                {
                    bookingId = b.Id,
                    name = b.Resident.FullName,
                    imageUrl = b.Resident.ProfilePhoto,
                    bookingTime = b.BookingDate,
                    serviceName = ((Package)b.Service).Name,
                    DurationInMonths = ((Package)b.Service).DurationInMonths,
                    bookingStatus = b.BookingStatus

                }).ToListAsync();
            
        }

        public Task<List<BookingOfUser>> PackagebookingOfResident(string residentId)
        {
            return _context.GymBooking.AsNoTracking().Where(b => b.ResidentId == residentId && b.Service is Package).Include(b => b.Gym).Include(b => b.Service)
                .Select(b => new BookingOfUser
                {
                    bookingId = b.Id,
                    GymName = b.Gym.FullName,
                    imageUrl = b.Gym.ProfilePhoto,
                    bookingTime = b.BookingDate,
                    serviceName = ((Package)b.Service).Name,
                    DurationInMonths = ((Package)b.Service).DurationInMonths,
                    bookingStatus = b.BookingStatus
                }).ToListAsync();
        }

        public async Task<List<BookingOfUser>> PackagebookingOfResidentAndStatus(string residentId, GymBookingStatus status)
        {
            await _context.GymBooking.AsNoTracking().Where(b => b.ResidentId == residentId && b.BookingStatus == status && b.Service is Package).Include(b => b.Gym)
                .Include(b => b.Service)
                .Select(b => new BookingOfUser
                {
                    bookingId = b.Id,
                    GymName = b.Gym.FullName,
                    imageUrl = b.Gym.ProfilePhoto,
                    bookingTime = b.BookingDate,
                    serviceName = ((Package)b.Service).Name,
                    DurationInMonths = ((Package)b.Service).DurationInMonths,
                    bookingStatus = b.BookingStatus
                }).ToListAsync();
            return null;
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
    }
}
