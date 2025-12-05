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

        [HttpPut("UpdateDoctorProfile")]
        public async Task<IActionResult> UpdateDoctorProfile([FromForm] UpdateDoctorDto doctorDto, string lan = "en")
        {
            await _doctorService.UpdateDoctorProfile(doctorDto);
            return Ok(ResponseHelper.Success("UpdateDoctorProfileSuccess", lan));
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

        [HttpGet("GetDoctorChart/{doctorId}")]
        public async Task<IActionResult> GetDoctorChart(string doctorId,string lan="en")
        {
            var doctorChart = await _doctorService.GetDoctorChart(doctorId);
            return Ok(ResponseHelper.Success("FetchDoctorChartSuccess", lan, doctorChart));
        }

        [HttpGet("GetAllBookingsOfDoctor/{doctorId}/{status}")]
        public async Task<IActionResult> GetAllBookingOfDoctors(string doctorId, BookingStatus status = BookingStatus.upcoming, string lan = "en")
        {
            var bookings = await _doctorService.GetAllBookingOfDoctors(doctorId,status,lan);
            return Ok(ResponseHelper.Success("FetchAllBookingOfDoctorsSuccess", lan, bookings));
        }

        [HttpGet("GetDoctorBySpecialist/{specialistId}")]
        public async Task<IActionResult> GetDoctorBySpecialist(int specialistId = 0, string lan = "en")
        {
            if (specialistId == 0)
            {
                var doctors = await _doctorService.GetAllDoctors(lan);
                return Ok(ResponseHelper.Success("FetchAllDoctorsSuccess", lan, doctors));
            }
            else
            {
                var doctors = await _doctorService.GetDoctorBySpecialist(specialistId, lan);
                return Ok(ResponseHelper.Success("FetchDoctorsBySpecialistSuccess", lan, doctors));
            }
        }
    }
}