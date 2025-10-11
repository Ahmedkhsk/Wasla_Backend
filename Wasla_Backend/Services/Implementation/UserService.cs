
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Wasla_Backend.Helpers;

namespace Wasla_Backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly EmailSenderHelper _emailSender;
        private readonly IMapper _mapper;
        private readonly TokenHelper _TokenHelper;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserService(IUserFactory userFactory, IUserRepository userRepository, IEmailVerificationRepository emailVerificationRepository, IRoleRepository roleRepository, EmailSenderHelper emailSender, IMapper mapper, TokenHelper tokenHelper, UserManager<ApplicationUser> userManager)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _emailVerificationRepository = emailVerificationRepository;
            _roleRepository = roleRepository;
            _emailSender = emailSender;
            _mapper = mapper;
            _TokenHelper = tokenHelper;
            _userManager = userManager;
        }


        
        public async Task<IdentityResult>CheckMailForVerficatio(CheckMailDto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                throw new NotFoundException("User not found.");
            if (!user.IsVerified)
                throw new BadRequestException("User is not verified.");
            if (!user.IsApproved)
                throw new BadRequestException("User is not approved by admin yet.");
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

            var loginResponse = new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Role = roles.FirstOrDefault()
            };
            return loginResponse;


        }


        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
                throw new BadRequestException("Email already exists.");

            var user = _userFactory.CreateUser(model.Role);

            _mapper.Map(model, user);
            
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


    }
}
