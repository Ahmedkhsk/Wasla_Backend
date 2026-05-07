namespace Wasla_Backend.Controllers.Gym
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GymController : ControllerBase
    {
        private readonly IGymService _gymService;

        public GymController(IGymService gymService)
        {
            _gymService = gymService;
        }

        [AllowAnonymous]
        [HttpPost("CompleteRegister")]
        public async Task<IActionResult> CompleteRegister(GymCompleteRegisterDto service,
                                                          [FromQuery] LanDto lanDto)
        {
            await _gymService.CompleteRegister(service);

            return Ok(ResponseHelper.Success(LocalizationKey.CompleteDataSuccess,
                                             lanDto.lan));
        }

        [HttpGet("AllGyms")]
        public async Task<IActionResult> AllGyms(int pageNumber = 1,
                                                 int pageSize = 10,
                                                 string lan = "en")
        {
            var data = await _gymService.AllGyms(pageNumber, pageSize);

            return Ok(ResponseHelper.Success(LocalizationKey.AllGymsData,
                                             lan,
                                             data));
        }

        [Authorize(Roles = "gym")]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileGym dto,
                                                       [FromQuery] LanDto lanDto)
        {
            await _gymService.UpdateProfile(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.UpdateProfileSuccess,
                                             lanDto.lan));
        }

        [HttpGet("GymProfile")]
        public async Task<IActionResult> GymProfile(string id,
                                                    [FromQuery] LanDto lanDto)
        {
            var data = await _gymService.GymProfile(id);

            return Ok(ResponseHelper.Success(LocalizationKey.GymProfileData,
                                             lanDto.lan,
                                             data));
        }
    }
}