using System.Linq;

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


        public UserService(IUserFactory userFactory, IUserRepository userRepository, IEmailVerificationRepository emailVerificationRepository, IRoleRepository roleRepository, EmailSenderHelper emailSender, IMapper mapper, TokenHelper tokenHelper, UserManager<ApplicationUser> userManager,IRefreshTokenRepository refreshTokenRepository)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _emailVerificationRepository = emailVerificationRepository;
            _roleRepository = roleRepository;
            _emailSender = emailSender;
            _mapper = mapper;
            _TokenHelper = tokenHelper;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
        }


        public async Task<IdentityResult> VerifyEmailAsync(VerificationEmailDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException("User not found.");
            if (user.IsVerified)
                throw new BadRequestException("User is already verified.");
            var verification = await _emailVerificationRepository.GetByEmailAndCodeAsync(model.Email,model.VerificationCode);
            if (verification == null || verification.ExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("Invalid or expired verification code.");
            user.IsVerified = true;
            var result = await _userRepository.UpdateUserAsync(user);
            if (!result.Succeeded)
                return result;
            await _emailVerificationRepository.RemoveAsync(verification);
            return result;
        }
        public async Task<IdentityResult>CheckMailForVerficatio(CheckMailDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException("User not found.");
           
            
            string verificationCode = new Random().Next(1000, 9999).ToString();
            await _emailSender.SendEmailAsync(
                        model.Email,
                        "Verification Code",
                        $"Your OTP is: <b>{verificationCode}</b>"
                    );
            await _emailVerificationRepository.AddVerificationAsync(model.Email, verificationCode, TimeSpan.FromMinutes(1));
            return IdentityResult.Success;
        }

        public async Task<IdentityResult>ForgetPasswordAsync(ForgetPasswordDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException("User not found.");
            if (!user.IsVerified)
                throw new BadRequestException("User is not verified.");
            if (!user.IsApproved)
                throw new BadRequestException("User is not approved by admin yet.");
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
                throw new NotFoundException("User not found.");

            if (!user.IsVerified)
                throw new BadRequestException("User is not verified.");
            if (!user.IsApproved)
                throw new BadRequestException("User is not approved by admin yet.");
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result;
        }

        public async Task<LoginResponse> LoginAsync(LoginDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException("User not found.");

            if (!user.IsVerified)
                throw new BadRequestException("User is not verified.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!user.IsApproved)
                throw new BadRequestException("User is not approved by admin yet.");

            if (!isPasswordValid)
                throw new BadRequestException("Password is incorrect.");

        

            var roles = await _roleRepository.GetUserRolesAsync(user);
            var token = _TokenHelper.GenerateToken(user, roles);
            var refreshToken = _TokenHelper.GenerateRefreshToken();

            var loginResponse = new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Role = roles.FirstOrDefault(),
                RefreshToken = refreshToken
            };
            var refreshtoken = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            await _refreshTokenRepository.AddAsync(refreshtoken);
            await _refreshTokenRepository.SaveChangesAsync();


            return loginResponse;


        }


        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
                throw new BadRequestException("Email already exists.");

            var user = _userFactory.CreateUser(model.Role);

            _mapper.Map(model, user);
            
            var roles = await _roleRepository.GetRolesNameAsync();

            if (!roles.Contains(model.Role))
                throw new NotFoundException("Role Not Found.");

            var result = await _userRepository.CreateUserAsync(user, model.Password);

            if (!result.Succeeded)
                return result;
            
            string verificationCode = new Random().Next(1000, 9999).ToString();

            await _emailSender.SendEmailAsync(
                        model.Email,
                        "Verification Code",
                        $"Your OTP is: <b>{verificationCode}</b>"
                    );

            await _emailVerificationRepository.AddVerificationAsync(model.Email,verificationCode,TimeSpan.FromMinutes(1));
       
            await _roleRepository.AddUserToRoleAsync(user, model.Role);

            return result;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDto model)
        {
           var refreshToken =await _refreshTokenRepository.GetByTokenAsync(model.RefreshToken);
            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("Invalid refresh token.");
            var user = await _userRepository.GetUserByIdAsync(refreshToken.UserId);
            if (user == null)
                throw new NotFoundException("User not found.");
            var roles = await _roleRepository.GetUserRolesAsync(user);
            var token = _TokenHelper.GenerateToken(user, roles);
            var newRefreshToken = _TokenHelper.GenerateRefreshToken();
            refreshToken.IsRevoked = true;
             _refreshTokenRepository.Update(refreshToken);
            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();
            var loginResponse = new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Role = roles.FirstOrDefault(),
                RefreshToken = newRefreshToken
            };
            return loginResponse;


        }


    }
}
