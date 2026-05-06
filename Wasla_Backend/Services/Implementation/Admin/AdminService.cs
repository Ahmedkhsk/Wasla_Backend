using Wasla_Backend.DTOs.PaginationDTOS;

namespace Wasla_Backend.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<ContactUs> _contatUsRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailSenderHelper _emailSenderHelper;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IOrderRepository _orderRepository;

        public AdminService(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IGenericRepository<ContactUs> contatUsRepository,
            IRoleRepository roleRepository,
            IEmailSenderHelper emailSenderHelper,
            IFileUrlBuilderService fileUrlBuilderService,
            IOrderRepository orderRepository
            )
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _contatUsRepository = contatUsRepository;
            _roleRepository = roleRepository;
            _emailSenderHelper = emailSenderHelper;
            _fileUrlBuilderService = fileUrlBuilderService;
            _orderRepository = orderRepository;
        }

        public async Task<AdminChartResponse> GetCollectedCountBookingsPerYear()
        {
            var countCompletedOrders = await _orderRepository.CountOrders(OrderStatus.Delivered);
            var countCanceledOrders = await _orderRepository.CountOrders(OrderStatus.Cancelled);
            var countCompletedBookings = await _bookingRepository.CountBookings(BookingStatus.completed);
            var countCanceledBookings = await _bookingRepository.CountBookings(BookingStatus.canceled);

            var bookingsPerYear = await _bookingRepository.GetCollectedPriceBookingsPerYear();
            var ordersPerYear = await _orderRepository.GetCollectedPriceOrdersPerYear();

            return new AdminChartResponse
            {
                completedBookingsCount = countCompletedOrders + countCompletedBookings,
                canceledBookingsCount = countCanceledOrders + countCanceledBookings,
                countOfUsers = await _userRepository.countUsers(),
                years = MergeCollectedPerYear(bookingsPerYear, ordersPerYear)
            };
        }

        public async Task ChangeUserStatus(ChangeUserStsatusDto changeUserStsatus)
        {
            var user = await _userRepository.GetUserByIdAsync(changeUserStsatus.userId);

            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            user.Status = changeUserStsatus.status;

            var result = await _userRepository.UpdateUserAsync(user);

            if (!result.Succeeded)
                throw new BadRequestException(LocalizationKey.FailedToChangeUserStatus);
            if(changeUserStsatus.status == UserStatus.Active)
            {
                var emailSubject = "Account Activated";
                var emailBody = $"Dear {user.FullName},\n\nYour account has been Activated. You can now log in and start using our services.\n\nBest regards,\nWasla Team";
                await _emailSenderHelper.SendEmailAsync(user.Email, emailSubject, emailBody);
            }
            if(changeUserStsatus.status == UserStatus.Suspended)
            {
                var emailSubject = "Account Suspended";
                var emailBody = $"Dear {user.FullName},\n\nWe regret to inform you that your account has been suspended due to a violation of our terms of service. If you believe this is a mistake, please contact our support team for further assistance.\n\nBest regards,\nWasla Team";
                await _emailSenderHelper.SendEmailAsync(user.Email, emailSubject, emailBody);
            }
            if(changeUserStsatus.status==UserStatus.Disabled)
            {
                var emailSubject = "Account Disabled";
                var emailBody = $"Dear {user.FullName},\n\nWe regret to inform you that your account has been disabled due to a violation of our terms of service. If you believe this is a mistake, please contact our support team for further assistance.\n\nBest regards,\nWasla Team";
                await _emailSenderHelper.SendEmailAsync(user.Email, emailSubject, emailBody);
            }
        }

        public async Task AddContact(ContactUsDto contactUsDto)
        {
            var contact = new ContactUs
            {
                email = contactUsDto.email,
                fullName = contactUsDto.fullName,
                message = contactUsDto.message
            };

            await _contatUsRepository.AddAsync(contact);
            await _contatUsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactUs>> GetContacts()
        {
            return await _contatUsRepository.GetAllAsync();
        }

        public async Task<PagedResult<UserApproveResponse>> UserApproveResponses(string roleId, int pageNumber, int pageSize)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            if (role == null)
                throw new NotFoundException(LocalizationKey.RoleNotFound);

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

        public async Task<AdminUserDetailsResponseDto> GetUserDetailsAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var role = await _roleRepository.GetUserRolesAsync(user);
            var userBase = new AdminUserBaseDetailsDto(user);
            userBase.profilePhoto =  _fileUrlBuilderService.GetMediaUrl(userBase.profilePhoto, MediaType.userImage);
            var roleName = role.FirstOrDefault();

            return user switch
            {
                Doctor doctor => BuildDoctorDetails(doctor, userBase, roleName),
                Resident resident => BuildResidentDetails(resident, userBase, roleName),
                Gym gym => BuildGymDetails(gym, userBase, roleName),
                Technician tech => BuildTechnicianDetails(tech, userBase, roleName),
                DriverModel driver => BuildDriverDetails(driver, userBase, roleName),
                Restaurant rest => BuildRestaurantDetails(rest, userBase, roleName),
                _ => throw new NotFoundException(LocalizationKey.UserNotFound)
            };
        }

        
        private AdminUserDetailsResponseDto BuildDoctorDetails(
            Doctor doctor, AdminUserBaseDetailsDto userBase, string? role)
        {
            var details = new AdminDoctorDetailsDto(doctor);
            details.CV = _fileUrlBuilderService.GetMediaUrl(details.CV, MediaType.doctorCV);
            return new AdminUserDetailsResponseDto { role = role, userBase = userBase, details = details };
        }

        private AdminUserDetailsResponseDto BuildResidentDetails(
            Resident resident, AdminUserBaseDetailsDto userBase, string? role)
        {
            var details = new AdminResidentDetailsDto(resident);

            return new AdminUserDetailsResponseDto { role = role, userBase = userBase, details = details };
        }

        private AdminUserDetailsResponseDto BuildGymDetails(
            Gym gym, AdminUserBaseDetailsDto userBase, string? role)
        {
            var details = new AdminGymDetailsDto(gym);
            details.images = details.images.Select(img => _fileUrlBuilderService.GetMediaUrl(img, MediaType.gymImage)).ToList();
            return new AdminUserDetailsResponseDto { role = role, userBase = userBase, details = details };
        }

        private AdminUserDetailsResponseDto BuildTechnicianDetails(
            Technician tech, AdminUserBaseDetailsDto userBase, string? role)
        {
            var details = new AdminTechnicianDetailsDto(tech);
            details.documents = details.documents.Select(doc => _fileUrlBuilderService.GetMediaUrl(doc, MediaType.TechnicianDocument)).ToList();
            return new AdminUserDetailsResponseDto { role = role, userBase = userBase, details = details };
        }

        private AdminUserDetailsResponseDto BuildDriverDetails(
            DriverModel driver, AdminUserBaseDetailsDto userBase, string? role)
        {
            var details = new AdminDriverDetailsDto(driver);
            details.CarImages = details.CarImages?.Select(img => _fileUrlBuilderService.GetMediaUrl(img, MediaType.DriverCarImage)).ToList();
            return new AdminUserDetailsResponseDto { role = role, userBase = userBase, details = details };
        }

        private AdminUserDetailsResponseDto BuildRestaurantDetails(
            Restaurant rest, AdminUserBaseDetailsDto userBase, string? role)
        {
            var details = new AdminRestaurantDetailsDto(rest);
            details.images = details.images.Select(img => _fileUrlBuilderService.GetMediaUrl(img, MediaType.restaurantImage)).ToList();
            return new AdminUserDetailsResponseDto { role = role, userBase = userBase, details = details };
        }

        private List<CollectedPerYearDto> MergeCollectedPerYear(
            List<CollectedPerYearDto> bookings,
            List<CollectedPerYearDto> orders)
        {
            var merged = bookings.ToDictionary(b => b.year);

            foreach (var order in orders)
            {
                if (!merged.TryGetValue(order.year, out var existing))
                {
                    merged[order.year] = order;
                    continue;
                }

                var monthDict = existing.months.ToDictionary(m => m.month);

                foreach (var month in order.months)
                {
                    if (monthDict.TryGetValue(month.month, out var existingMonth))
                        existingMonth.amount += month.amount;
                    else
                        existing.months.Add(month);
                }

                existing.months = existing.months.OrderBy(m => m.month).ToList();
            }

            return merged.Values.OrderBy(y => y.year).ToList();
        }
    }
}