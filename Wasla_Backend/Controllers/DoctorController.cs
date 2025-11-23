namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost("CompleteData")]
        public async Task<IActionResult> CompleteData([FromForm] DoctorCompleteDto doctorCompleteDto, string lan = "en")
        {
            await _doctorService.CompleteData(doctorCompleteDto);
            return Ok(ResponseHelper.Success("CompleteDataSuccess", lan));
        }

        [HttpGet("DoctorSpecializations")]
        public async Task<IActionResult> DoctorSpecializations(string lan = "en")
        {
            var specializations = await _doctorService.DoctorSpecializations(lan);
            return Ok(ResponseHelper.Success("FetchDoctorSpecializationsSuccess", lan, specializations));
        }

        [HttpGet("GetDoctorProfile/{id}")]
        public async Task<IActionResult> GetDoctorProfile(string id, string lan = "en")
        {
            var doctorProfiles = await _doctorService.GetDoctorProfile(id, lan);
            return Ok(ResponseHelper.Success("FetchDoctorProfileSuccess", lan, doctorProfiles));
        }
    }
}
