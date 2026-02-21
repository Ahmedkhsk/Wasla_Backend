using Wasla_Backend.Enums;

namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
    }
}