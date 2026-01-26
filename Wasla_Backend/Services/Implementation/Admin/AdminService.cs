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
                completedBookingsCount = await _bookingRepository.CountBookings(BookingStatus.completed),
                canceledBookingsCount = await _bookingRepository.CountBookings(BookingStatus.canceled),
                years = await _bookingRepository.GetCollectedPriceBookingsPerYear()
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
            return  await _contatUsRepository.GetAllAsync();
        }

        public async Task<PagedResult<UserApproveResponse>> UserApproveResponses(string roleName,int pageNumber, int pageSize)
        {
            var users = await _userRepository.GetUsersByRoleAsync(roleName);

            var totalCount = users.Count();

            var pagedUsers = users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new UserApproveResponse
                {
                    id = user.Id,
                    name = user.FullName,
                    email = user.Email,
                    status = user.Status,
                    CreatedAt = user.CreatedAt
                })
                .ToList();

            return new PagedResult<UserApproveResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = pagedUsers
            };
        }

    }
}
