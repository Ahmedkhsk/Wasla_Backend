namespace Wasla_Backend.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<ContactUs> _contatUsRepository;

        public AdminService(IBookingRepository bookingRepository, IUserRepository userRepository, IGenericRepository<ContactUs> contatUsRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _contatUsRepository = contatUsRepository;
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

        public async Task AddContact(ContactUsDto contactUsDto)
        {
            var contact = new ContactUs();
            contact.email = contactUsDto.email;
            contact.fullName = contactUsDto.fullName;
            contact.message = contactUsDto.message;

            await _contatUsRepository.AddAsync(contact);
            await _contatUsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactUs>> GetContacts()
        {
            var res =  await _contatUsRepository.GetAllAsync();
            
            if (res == null)
                res = [];
            
            return res;
        }
    
    }
}
