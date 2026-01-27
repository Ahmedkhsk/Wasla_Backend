namespace Wasla_Backend.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<ContactUs> _contatUsRepository;
        private readonly IRoleRepository _roleRepository;

        public AdminService(IBookingRepository bookingRepository, IUserRepository userRepository, 
            IGenericRepository<ContactUs> contatUsRepository,IRoleRepository roleRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _contatUsRepository = contatUsRepository;
            _roleRepository = roleRepository;
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

        public async Task<PagedResult<UserApproveResponse>> UserApproveResponses(string roleId, int pageNumber, int pageSize)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            if(role == null)
                throw new NotFoundException("RoleNotFound");
            
            var users = await _userRepository.GetUsersByRoleAsync(role.Name);

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
