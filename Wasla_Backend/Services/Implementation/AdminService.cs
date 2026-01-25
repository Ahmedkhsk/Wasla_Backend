namespace Wasla_Backend.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public AdminService(IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }

        public async Task<AdminChartResponse> GetCollectedCountBookingsPerYear(BookingStatus status)
        {
            return new AdminChartResponse 
            {  
                collectedBookings = await _bookingRepository.GetCollectedCountBookingsPerYear(status)
            };
        }

        public async Task ChangeUserStatus(ChangeUserStsatusDto changeUserStsatus)
        {
            var user = await _userRepository.GetUserByIdAsync(changeUserStsatus.userId);
            if (user == null)
            {
                throw new NotFoundException("UserNotFound");
            }
            user.Status = changeUserStsatus.status;
            var result = await _userRepository.UpdateUserAsync(user);
            if (!result.Succeeded)
            {
                throw new BadRequestException("FailedToChangeUserStatus");
            }
        }
    }
}
