using Wasla_Backend.Enums;

namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Context _context;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, Context context,IConfiguration configuration)
        {
            _userService = userService;
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromBody] TokenDto dto, [FromQuery] LanDto lanDto)
        {
            if (string.IsNullOrEmpty(dto.token))
                return Ok(ResponseHelper.Success(LocalizationKey.InvalidToken, lanDto.lan, false));

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);

                tokenHandler.ValidateToken(dto.token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:Audience"],

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                }, out _);

                return Ok(ResponseHelper.Success(LocalizationKey.TokenValid, lanDto.lan, true));
            }
            catch
            {
                return Ok(ResponseHelper.Success(LocalizationKey.InvalidToken, lanDto.lan, false));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidData, lan,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var response = await _userService.LoginAsync(model);
            return Ok(ResponseHelper.Success(LocalizationKey.LoginSuccess, lan, response));
        }

        [HttpPost("register")]
        public async Task<IActionResult> PreRegister(RegisterDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidRequest, lan,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            if (model.Password != model.ConfirmPassword)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.PassMismatch, lan));

            var result = await _userService.RegisterAsync(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.RegistrationFailed, lan, result.Errors));

            var returnModel = new
            {
                model.Email,
                model.roleId
            };

            return Ok(ResponseHelper.Success(LocalizationKey.RegistrationSuccess, lan, returnModel));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidRequest, lan,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var result = await _userService.ChangePasswordAsync(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.ChangePasswordFailed, lan, result.Errors));

            return Ok(ResponseHelper.Success(LocalizationKey.ChangePassSuccess, lan));
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerificationEmailDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidRequest, lan,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var result = await _userService.VerifyEmailAsync(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.EmailVerificationFailed, lan, result.Errors));

            return Ok(ResponseHelper.Success(LocalizationKey.EmailVerified, lan, result));
        }

        [HttpPost("approve-verify")]
        public async Task<IActionResult> ApproveAndVerify([FromQuery] string gmail, string lan = "en")
        {
            await _userService.approveAndVerify(gmail);
            return Ok();
        }

        [HttpPost("check-mail-verification")]
        public async Task<IActionResult> CheckMailForVerification([FromBody] CheckMailDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidRequest, lan,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var result = await _userService.CheckMailForVerficatio(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.verficationEmailFailed, lan, result.Errors));

            return Ok(ResponseHelper.Success(LocalizationKey.verficationEmailSent, lan, result));
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidData, lan,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var result = await _userService.ForgetPasswordAsync(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.ChangePassFailed, lan, result.Errors));

            return Ok(ResponseHelper.Success(LocalizationKey.ChangePassSuccess, lan, result));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidData, lan,
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var response = await _userService.RefreshTokenAsync();

            if (response == null)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidToken, lan));

            return Ok(ResponseHelper.Success(LocalizationKey.TokenRefreshSuccess, lan, response));
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> AllUsers(string lan = "en")
        {
            var response = await _userService.AllUsers();
            return Ok(ResponseHelper.Success(LocalizationKey.GetAllUsersSuccess, lan, response));
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser(string gmail, string lan = "en")
        {
            await _userService.Delete(gmail);
            return Ok(ResponseHelper.Success(LocalizationKey.DeleteUserSuccess, lan));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string lan = "en")
        {
            await _userService.Logout();
            return Ok(ResponseHelper.Success(LocalizationKey.UserLoggedOutSuccess, lan));
        }

        [HttpPut("phone")]
        public async Task<IActionResult> UpdatePhoneNumber(string gmail, string phone)
        {
         _context.Users.Where(u=>u.Email == gmail).ExecuteUpdate(s => s.SetProperty(u => u.PhoneNumber, phone));
            return Ok();
        }
    }
}