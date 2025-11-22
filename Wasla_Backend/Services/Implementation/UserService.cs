namespace Wasla_Backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly EmailSenderHelper _emailSender;
        private readonly IMapper _mapper;
        private readonly TokenHelper _TokenHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _imagePath;

        public UserService(
            IUserFactory userFactory,
            IUserRepository userRepository,
            IEmailVerificationRepository emailVerificationRepository,
            IRoleRepository roleRepository,
            EmailSenderHelper emailSender,
            IMapper mapper,
            TokenHelper tokenHelper,
            UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepository,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _emailVerificationRepository = emailVerificationRepository;
            _roleRepository = roleRepository;
            _emailSender = emailSender;
            _mapper = mapper;
            _TokenHelper = tokenHelper;
            _userManager = userManager;
            Console.WriteLine("WWWROOT = " + webHostEnvironment.WebRootPath);

            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathUser.TrimStart('/'));
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<IdentityResult> VerifyEmailAsync(VerificationEmailDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            var verification = await _emailVerificationRepository.GetByEmailAndCodeAsync(model.Email, model.VerificationCode);
            if (verification == null || verification.ExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("InvalidOrExpiredCode");

            user.IsVerified = true;
            var result = await _userRepository.UpdateUserAsync(user);
            if (!result.Succeeded)
                return result;

            await _emailVerificationRepository.RemoveAsync(verification);
            return result;
        }

        public async Task<IdentityResult> CheckMailForVerficatio(CheckMailDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException("UserNotFound");

            string verificationCode = new Random().Next(1000, 9999).ToString();
            await _emailSender.SendEmailAsync(model.Email, "Verification Code", $"Your OTP is: <b>{verificationCode}</b>");
            await _emailVerificationRepository.AddVerificationAsync(model.Email, verificationCode, TimeSpan.FromMinutes(1));
            return IdentityResult.Success;
        }

        public async Task approveAndVerify(string gmail)
        {
            var user = await _userRepository.GetUserByEmailAsync(gmail);
            user.IsApproved = true;
            user.IsVerified = true;
            user.Status = 0;
            await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ForgetPasswordAsync(ForgetPasswordDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException("UserNotFound");

           

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
                throw new NotFoundException("UserNotFound");

            if (!user.IsVerified)
                throw new BadRequestException("UserNotVerified");

            if (!user.IsApproved)
                throw new BadRequestException("UserNotApproved");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result;
        }

        public async Task<LoginResponse> LoginAsync(LoginDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException("EmailNotFound");


            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        

            if (!isPasswordValid)
                throw new BadRequestException("IncorrectPassword");

            var roles = await _roleRepository.GetUserRolesAsync(user);
            var token = _TokenHelper.GenerateToken(user, roles);
            var refreshToken = _TokenHelper.GenerateRefreshToken();

            var loginResponse = new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Role = roles.FirstOrDefault(),
                RefreshToken = refreshToken,
                IsCompletedRegister=user.IsCompleteRegistration,
                IsVerfied=user.IsVerified
                ,statue=user.Status

            };

            var refreshtoken = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(refreshtoken);
            await _refreshTokenRepository.SaveChangesAsync();

            return loginResponse;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
                throw new BadRequestException("EmailExists");

            var user = _userFactory.CreateUser(model.Role);
            _mapper.Map(model, user);

            var roles = await _roleRepository.GetRolesNameAsync();
            if (!roles.Contains(model.Role))
                throw new NotFoundException("RoleNotFound");

            var result = await _userRepository.CreateUserAsync(user, model.Password);
            if (!result.Succeeded)
                return result;

            string verificationCode = new Random().Next(1000, 9999).ToString();
            await _emailSender.SendEmailAsync(model.Email, "Verification Code", $"Your OTP is: <b>{verificationCode}</b>");
            await _emailVerificationRepository.AddVerificationAsync(model.Email, verificationCode, TimeSpan.FromMinutes(1));
            await _roleRepository.AddUserToRoleAsync(user, model.Role);

            return result;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto model)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(model.RefreshToken);
            if (refreshToken == null)
                throw new BadRequestException("InvalidRefreshToken");

            if (refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                _refreshTokenRepository.Delete(refreshToken);
                await _refreshTokenRepository.SaveChangesAsync();
                throw new BadRequestException("ExpiredRefreshToken");
            }

            var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId);
            if (user == null)
                throw new NotFoundException("UserNotFound");

            var roles = await _roleRepository.GetUserRolesAsync(user);
            var token = _TokenHelper.GenerateToken(user, roles);

            return new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Role = roles.FirstOrDefault(),
                RefreshToken = refreshToken.Token
            };
        }



        public async Task<object> AllUsers()
        {
           var users= await _userRepository.GetAll();
            return users.Select(u => new
            {
                u.Id,
                u.Email,

            });
        }
        public async Task Delete(string gmail)
        {
            var user = await _userRepository.GetUserByEmailAsync(gmail);
            if (user == null)
                throw new NotFoundException("UserNotFound");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

    }
}
