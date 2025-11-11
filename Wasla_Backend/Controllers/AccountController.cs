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
                return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var response = await _userService.LoginAsync(model);
            return Ok(ResponseHelper.Success("LoginSuccess", lan, response));
        }

        [HttpPost("register")]
        public async Task<IActionResult> PreRegister(RegisterDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidRequest", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            if (model.Password != model.ConfirmPassword)
                return BadRequest(ResponseHelper.Fail("PassMismatch", lan));

            var result = await _userService.RegisterAsync(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail("RegistrationFailed", lan, result.Errors));

            var returnModel = new
            {
                model.Email,
                model.Role
            };

            return Ok(ResponseHelper.Success("RegistrationSuccess", lan, returnModel));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidRequest", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var result = await _userService.ChangePasswordAsync(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail("ChangePasswordFailed", lan, result.Errors));

            return Ok(ResponseHelper.Success("ChangePassSuccess", lan));
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerificationEmailDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidRequest", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var result = await _userService.VerifyEmailAsync(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail("EmailVerificationFailed", lan, result.Errors));

            return Ok(ResponseHelper.Success("EmailVerified", lan, result));
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
                return BadRequest(ResponseHelper.Fail("InvalidRequest", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var result = await _userService.CheckMailForVerficatio(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail("VerificationEmailFailed", lan, result.Errors));

            return Ok(ResponseHelper.Success("VerificationEmailSent", lan, result));
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var result = await _userService.ForgetPasswordAsync(model);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail("ChangePassFailed", lan, result.Errors));

            return Ok(ResponseHelper.Success("ChangePassSuccess", lan, result));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto model, string lan = "en")
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var response = await _userService.RefreshTokenAsync(model);

            if (response == null)
                return BadRequest(ResponseHelper.Fail("InvalidToken", lan));

            return Ok(ResponseHelper.Success("TokenRefreshSuccess", lan, response));
        }
    }
}
