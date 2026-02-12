

namespace Wasla_Backend.Repositories.Implementation.Gyms
{
    public class GymBookingRepository: GenericRepository<GymBooking>, IGymBookingRepository
    {
        public GymBookingRepository(Context context) : base(context)
        {
        }

        public async Task<List<BookingOfGym>> PackagebookingOfGym(string gymId)
        {
            return await _context.GymBooking.Where(b => b.GymId == gymId).Include(b=>b.Resident).Include(b=>b.Service)
                .Select(b => new BookingOfGym
                {
                  bookingId=b.Id,
                  name=b.Resident.FullName,
                  imageUrl=b.Resident.ProfilePhoto,
                  bookingTime = b.BookingDate,
                  serviceName=((Package)b.Service).Name,
                  DurationInMonths=((Package)b.Service).DurationInMonths,
                  bookingStatus=b.BookingStatus

                }).ToListAsync();
                ;
        }

        public async Task<List<BookingOfGym>> PackagebookingOfGymAndStatus(string gymId, GymBookingStatus status)
        {
            return await _context.GymBooking.Where(b => b.GymId == gymId&&b.BookingStatus==status)
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
            return _context.GymBooking.Where(b => b.ResidentId == residentId).Include(b => b.Gym).Include(b => b.Service)
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
            await _context.GymBooking.Where(b => b.ResidentId == residentId && b.BookingStatus == status).Include(b => b.Gym)
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
    }
}
