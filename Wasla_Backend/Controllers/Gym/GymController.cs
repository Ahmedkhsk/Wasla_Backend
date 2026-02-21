namespace Wasla_Backend.Controllers.Gym
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymController : ControllerBase
    {
        private readonly IGymService _gymService;

        public GymController(IGymService gymService)
        {
            _gymService = gymService;
        }

        [HttpPost("CompleteRegister")]
        public async Task<IActionResult> CompleteRegister(GymCompleteRegisterDto service, string lan = "en")
        {
            await _gymService.CompleteRegister(service);
            return Ok(ResponseHelper.Success(LocalizationKey.CompleteDataSuccess, lan));
        }

        [HttpGet("AllGyms")]
        public async Task<IActionResult> AllGyms(int pageNumber = 1, int pageSize = 10, string lan = "en")
        {
            var data = await _gymService.AllGyms(pageNumber, pageSize);
            return Ok(ResponseHelper.Success(LocalizationKey.AllGymsData, lan, data));
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileGym dto, string lan = "en")
        {
            await _gymService.UpdateProfile(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.UpdateProfileSuccess, lan));
        }

        [HttpGet("GymProfile")]
        public async Task<IActionResult> GymProfile(string id, string lan = "en")
        {
            var data = await _gymService.GymProfile(id);
            return Ok(ResponseHelper.Success(LocalizationKey.GymProfileData, lan, data));
        }
    }
}