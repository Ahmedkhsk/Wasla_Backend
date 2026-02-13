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
            return Ok(ResponseHelper.Success("CompleteDataSuccess", lan));
        }
        [HttpGet("AllGyms")]
        public async Task<IActionResult> AllGyms(int pageNumber = 1, int pageSize = 10,string lan = "en")
        {
            var data = await _gymService.AllGyms(pageNumber, pageSize);
            return Ok(ResponseHelper.Success("AllGymsData", lan, data));
        }
        [HttpGet("GymProfile")]
        public async Task<IActionResult> GymProfile(string id, string lan = "en")
        {
            var data = await _gymService.GymProfile(id);
            return Ok(ResponseHelper.Success("GymProfileData", lan, data));
        }
    }
}
