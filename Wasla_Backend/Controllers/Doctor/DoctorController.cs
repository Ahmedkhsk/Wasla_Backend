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
            return Ok(ResponseHelper.Success(LocalizationKey.CompleteDataSuccess, lan));
        }

        [HttpPut("UpdateDoctorProfile")]
        [Authorize(Roles = "doctor")]

        public async Task<IActionResult> UpdateDoctorProfile([FromForm] UpdateDoctorDto doctorDto, string lan = "en")
        {
            await _doctorService.UpdateDoctorProfile(doctorDto);
            return Ok(ResponseHelper.Success(LocalizationKey.UpdateDoctorProfileSuccess, lan));
        }
        [HttpGet("DoctorSpecializations")]
        [AllowAnonymous]
        public async Task<IActionResult> DoctorSpecializations(string lan = "en")
        {
            var specializations = await _doctorService.DoctorSpecializations(lan);
            return Ok(ResponseHelper.Success(LocalizationKey.FetchDoctorSpecializationsSuccess, lan, specializations));
        }

        [HttpGet("GetDoctorProfile/{id}")]
        [Authorize(Roles = "doctor,resident")] 
        public async Task<IActionResult> GetDoctorProfile(string id, string lan = "en")
        {
            var doctorProfiles = await _doctorService.GetDoctorProfile(id, lan);
            return Ok(ResponseHelper.Success(LocalizationKey.FetchDoctorProfileSuccess, lan, doctorProfiles));
        }

        [HttpGet("GetDoctorChart/{doctorId}")]
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> GetDoctorChart(string doctorId, string lan = "en")
        {
            var doctorChart = await _doctorService.GetDoctorChart(doctorId);
            return Ok(ResponseHelper.Success(LocalizationKey.FetchDoctorChartSuccess, lan, doctorChart));
        }

        [HttpGet("GetAllBookingsOfDoctor/{doctorId}/{status}")]
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> GetAllBookingOfDoctors(string doctorId, BookingStatus status = BookingStatus.upcoming, string lan = "en")
        {
            var bookings = await _doctorService.GetAllBookingOfDoctors(doctorId, status, lan);
            return Ok(ResponseHelper.Success(LocalizationKey.FetchAllBookingOfDoctorsSuccess, lan, bookings));
        }

        [HttpGet("GetDoctorBySpecialist/{specialistId}")]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> GetDoctorBySpecialist(int specialistId = 0, string lan = "en")
        {
            if (specialistId == 0)
            {
                var doctors = await _doctorService.GetAllDoctors(lan);
                return Ok(ResponseHelper.Success(LocalizationKey.FetchAllDoctorsSuccess, lan, doctors));
            }
            else
            {
                var doctors = await _doctorService.GetDoctorBySpecialist(specialistId, lan);
                return Ok(ResponseHelper.Success(LocalizationKey.FetchDoctorsBySpecialistSuccess, lan, doctors));
            }
        }
        [HttpGet("GetDoctorData/{doctorId}")]
        [Authorize(Roles = "doctor,resident")]
        public async Task<IActionResult> GetDoctorData(string doctorId, string lan = "en")
        {
            var doctorData = await _doctorService.GetDoctorData(doctorId, lan);
            return Ok(ResponseHelper.Success(LocalizationKey.FetchDoctorDataSuccess, lan, doctorData));
        }
    }
}