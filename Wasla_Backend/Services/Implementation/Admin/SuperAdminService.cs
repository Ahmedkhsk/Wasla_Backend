namespace Wasla_Backend.Services.Implementation
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public SuperAdminService(
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task AddAdminAsync(AddAdminDto dto)
        {
            var existing = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existing != null)
                throw new BadRequestException(LocalizationKey.EmailExists);

            var admin = new Admin
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Gender = dto.Gender,
                Phone = dto.Phone,
                IsVerified = true,
                IsCompleteRegistration = true,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow,
                isOnline = false
            };

            var result = await _userRepository.CreateUserAsync(admin, dto.Password);
            if (!result.Succeeded)
                throw new BadRequestException(LocalizationKey.RegistrationFailed);

            await _roleRepository.AddUserToRoleAsync(admin, "admin");
        }

        public async Task<IEnumerable<AdminResponseDto>> GetAllAdminsAsync()
        {
            var admins = await _userRepository.GetUsersByRoleAsync("admin");

            return admins.Where(a=>!a.IsDeleted).Select(a => new AdminResponseDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email,
                Phone = a.Phone,
                Status = a.Status
            });
        }

        public async Task RemoveAdminAsync(string adminId)
        {
            var admin = await _userRepository.GetUserByIdAsync(adminId);
            if (admin == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            admin.IsDeleted = true;

            var result = await _userRepository.UpdateUserAsync(admin);
            if (!result.Succeeded)
                throw new BadRequestException(LocalizationKey.FailedToDeleteAdmin);
        }
        public async Task ToggleAdminStatusAsync(string adminId)
        {
            var admin = await _userRepository.GetUserByIdAsync(adminId);
            if (admin == null )
                throw new NotFoundException(LocalizationKey.UserNotFound);

            admin.Status = admin.Status == UserStatus.Active
                ? UserStatus.Suspended
                : UserStatus.Active;

            var result = await _userRepository.UpdateUserAsync(admin);
            if (!result.Succeeded)
                throw new BadRequestException(LocalizationKey.FailedToChangeUserStatus);
        }
    }
}