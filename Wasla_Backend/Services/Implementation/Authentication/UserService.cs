namespace Wasla_Backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IEmailSenderHelper _emailSender;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _TokenHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ICacheManager _cacheManager;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public UserService(
            IUserFactory userFactory,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            EmailSenderHelper emailSender,
            IMapper mapper,
            TokenHelper tokenHelper,
            UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepository,
            IHttpContextAccessor httpContextAccessor,
            DateTimeHelper dateTimeHelper,
            CacheManager cacheManager,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService
        )
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _emailSender = emailSender;
            _mapper = mapper;
            _TokenHelper = tokenHelper;
            _userManager = userManager;
            _dateTimeHelper = dateTimeHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
            _cacheManager = cacheManager;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<IdentityResult> VerifyEmailAsync(VerificationEmailDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            string cacheKey = $"verify:{user.Id}";
            var cachedCode = _cacheManager.Get<string>(cacheKey);

            if (string.IsNullOrEmpty(cachedCode) || cachedCode != model.VerificationCode)
                throw new BadRequestException(LocalizationKey.InvalidOrExpiredCode);

            _cacheManager.Remove(cacheKey);

            user.IsVerified = true;
            var result = await _userRepository.UpdateUserAsync(user);

            return result;
        }

        public async Task<IdentityResult> CheckMailForVerficatio(CheckMailDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            string verificationCode = new Random().Next(1000, 9999).ToString();
            await _emailSender.SendEmailAsync(model.Email, "Verification Code", $"Your OTP is: <b>{verificationCode}</b>");

            string cacheKey = $"verify:{user.Id}";
            _cacheManager.Set(cacheKey, verificationCode, TimeSpan.FromMinutes(1));

            return IdentityResult.Success;
        }

        public async Task approveAndVerify(string gmail)
        {
            var user = await _userRepository.GetUserByEmailAsync(gmail);
            user.Status = UserStatus.Active;
            user.IsVerified = true;
            await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ForgetPasswordAsync(ForgetPasswordDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
                return result;

            result = await _userManager.AddPasswordAsync(user, model.NewPassword);

            return result;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            if (!user.IsVerified)
                throw new BadRequestException(LocalizationKey.UserNotVerified);

            if (user.Status != UserStatus.Active)
                throw new BadRequestException(LocalizationKey.UserNotApproved);

            var isSameAsOld = await _userManager.CheckPasswordAsync(user, model.NewPassword);
            if (isSameAsOld)
                throw new BadRequestException(LocalizationKey.NewPasswordSameAsOld);

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result;
        }

        public async Task<LoginResponse> LoginAsync(LoginDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null||user.IsDeleted)
                throw new NotFoundException(LocalizationKey.EmailNotFound);
          

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
                throw new BadRequestException(LocalizationKey.IncorrectPassword);

            var roles = await _roleRepository.GetUserRolesAsync(user);
            var token = _TokenHelper.GenerateToken(user, roles);
            var refreshToken = _TokenHelper.GenerateRefreshToken();

            var loginResponse = new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                profilePhoto = _fileUrlBuilderService.GetMediaUrl(user.ProfilePhoto, MediaType.userImage),
                Role = roles.FirstOrDefault(),
                IsCompletedRegister = user.IsCompleteRegistration,
                IsVerfied = user.IsVerified,
                statue = user.Status
            };

            var refreshtoken = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(refreshtoken);
            await _refreshTokenRepository.SaveChangesAsync();

            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            return loginResponse;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
                throw new BadRequestException(LocalizationKey.EmailExists);

            var role = await _roleRepository.GetRoleByIdAsync(model.roleId);
            if (role == null)
                throw new NotFoundException(LocalizationKey.RoleNotFound);

            var user = _userFactory.CreateUser(role.Name);
            _mapper.Map(model, user);
            user.CreatedAt = _dateTimeHelper.Now;

            var result = await _userRepository.CreateUserAsync(user, model.Password);
            if (!result.Succeeded)
                return result;

            string verificationCode = new Random().Next(1000, 9999).ToString();
            await _emailSender.SendEmailAsync(model.Email, "Verification Code", $"Your OTP is: <b>{verificationCode}</b>");

            string cacheKey = $"verify:{user.Id}";
            _cacheManager.Set(cacheKey, verificationCode, TimeSpan.FromMinutes(1));
            await _roleRepository.AddUserToRoleAsync(user, role.Name);

            return result;
        }

        public async Task<LoginResponse> RefreshTokenAsync()
        {
            var refreshTokenCookie = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshTokenCookie);

            if (string.IsNullOrEmpty(refreshTokenCookie))
                throw new BadRequestException(LocalizationKey.RefreshTokenMissing);

            if (refreshToken == null)
                throw new BadRequestException(LocalizationKey.InvalidRefreshToken);

            if (refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                _refreshTokenRepository.Delete(refreshToken);
                await _refreshTokenRepository.SaveChangesAsync();
                throw new BadRequestException(LocalizationKey.ExpiredRefreshToken);
            }

            var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var roles = await _roleRepository.GetUserRolesAsync(user);
            var token = _TokenHelper.GenerateToken(user, roles);

            return new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Role = roles.FirstOrDefault(),
                IsCompletedRegister = user.IsCompleteRegistration,
                IsVerfied = user.IsVerified,
                statue = user.Status
            };
        }

        public async Task<object> AllUsers()
        {
            var users = await _userRepository.GetAll();
            return users.Select(u => new
            {
                u.Id,
                u.Email,
                type = u.GetType().Name,
            });
        }

        public async Task Delete(string gmail)
        {
            var user = await _userRepository.GetUserByEmailAsync(gmail);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
                await _userManager.RemoveFromRolesAsync(user, roles);

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task Logout()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException(LocalizationKey.UserNotLoggedIn);

            await _refreshTokenRepository.DeleteTokensByUserIdAsync(userId);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("RefreshToken");
        }
    }
}