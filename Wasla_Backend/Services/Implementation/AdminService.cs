namespace Wasla_Backend.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IBookingRepository _bookingRepository;

        public AdminService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<AdminChartResponse> GetCollectedCountBookingsPerYear(BookingStatus status)
        {
            return new AdminChartResponse 
            {  
                collectedBookings = await _bookingRepository.GetCollectedCountBookingsPerYear(status)
            };
        }
    }
}
