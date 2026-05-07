namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "resident")]
    public class ResidentController : ControllerBase
    {
        public IResidentService _residentService;
        public IResidentIdentityRepository _residentIdentityRepository;
        public IUserAuthorizationService _userAuthorizationService;

        public ResidentController(IResidentService residentService, IResidentIdentityRepository residentRepository, IUserAuthorizationService userAuthorizationService)
        {
            _residentService = residentService;
            _residentIdentityRepository = residentRepository;
            _userAuthorizationService = userAuthorizationService;
        }

        [AllowAnonymous]
        [HttpPost("CompleteRegister")]
        public async Task<IActionResult> CompleteRegister([FromForm] ResidentCompleteRegisterDto model, [FromQuery] LanDto lanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidData, lanDto.lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            await _residentService.CompleteResidentRegister(model);
            return Ok(ResponseHelper.Success(LocalizationKey.CompleteResidentRegisterSuccess, lanDto.lan));
        }

        [HttpPost("UploadIdentity")]
        [Authorize(Roles = "admin,superadmin")]
        public async Task AddIdentity(string NationalId, string gmail)
        {
            await _residentService.UploadIdentity(NationalId, gmail);
        }

        [HttpPut("edit-Profile")]
        public async Task<IActionResult> EditProfile(EditProfileDto editProfileDto, [FromQuery] LanDto lanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidData, lanDto.lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            await _residentService.EditProfile(editProfileDto);
            return Ok(ResponseHelper.Success(LocalizationKey.ProfileEditSuccess, lanDto.lan));
        }

        [HttpGet("get-Profile")]
        public async Task<IActionResult> GetProfile(string userId, [FromQuery]LanDto lanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidData, lanDto.lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var response = await _residentService.GetProfile(userId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetProfileSuccess, lanDto.lan, response));
        }

        [HttpGet("resident-chart")]
        public async Task<IActionResult> GetResidentChart(string residentId, [FromQuery] LanDto lanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.Fail(LocalizationKey.InvalidData, lanDto.lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

            var response = await _residentService.GetResidentChartAsync(residentId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetResidentChartSuccess, lanDto.lan, response));
        }
    }
}