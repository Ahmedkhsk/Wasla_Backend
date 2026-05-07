using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Wasla_Backend.Controllers.Technician
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianBookingController : ControllerBase
    {
        private readonly ITechnicianBookingService _technicianBookingService;

        public TechnicianBookingController(ITechnicianBookingService technicianBookingService)
        {
            _technicianBookingService = technicianBookingService;
        }

        [HttpPost("CreateBooking")]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> CreateBooking(TechnicianBookingRequestDto request, string lan = "en")
        {
            var result= await _technicianBookingService.RequestBooking(request);
            return Ok(ResponseHelper.Success(LocalizationKey.CreateBookingSuccessfully, lan,result));
        }

        [HttpGet("GetBookingDetailsForTechnician/{bookingId}")]
        [Authorize(Roles = "technician")]
        public async Task<IActionResult> GetBookingDetailsForTechnician(int bookingId, string lan = "en")
        {
            var result = await _technicianBookingService.GetBookingDetailsForTechnician(bookingId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetBookingDetailsSuccessfully, lan, result));
        }

        [HttpPut("accept/{bookingId}")]
        [Authorize(Roles = "technician")]
        public async Task<IActionResult> AcceptBooking(int bookingId, string lan = "en")
        {
            await _technicianBookingService.AcceptBooking(bookingId);
            return Ok(ResponseHelper.Success(LocalizationKey.AcceptBookingSuccessfully, lan));
        }

        [HttpPut("reject/{bookingId}")]
        [Authorize(Roles = "technician")]
        public async Task<IActionResult> RejectBooking(int bookingId, string lan = "en")
        {
            await _technicianBookingService.RejectBooking(bookingId);
            return Ok(ResponseHelper.Success(LocalizationKey.RejectBookingSuccessfully, lan));
        }

        [HttpPut("cancel/{bookingId}")]
        [Authorize(Roles = "technician,resident")]
        public async Task<IActionResult> CancelBooking(int bookingId,bool IsResident, string lan = "en")
        {
            await _technicianBookingService.CancelBooking(bookingId,IsResident);
            return Ok(ResponseHelper.Success(LocalizationKey.CancelBookingSuccessfully, lan));
        }

        [HttpGet("GetTechnicianBookings/{technicianId}")]
        [Authorize(Roles = "technician")]
        public async Task<IActionResult> GetTechnicianBookings(string technicianId, string lan = "en")
        {
            var result = await _technicianBookingService.technicianBookingOfTechnician(technicianId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetTechnicianBookingsSuccessfully, lan, result));
        }

        [HttpGet("GetResidentBookings/{residentId}")]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> GetResidentBookings(string residentId, string lan = "en")
        {
            var result = await _technicianBookingService.technicianBookingOfResidents(residentId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetResidentBookingsSuccessfully, lan, result));
        }
        [HttpGet("GetResidentBookingsBySpecialization/{residentId}/{specialization}")]
        [Authorize(Roles = "resident")]
        public async Task<IActionResult> GetResidentBookingsBySpecialization(string residentId, TechnicianSpecialty specialization, string lan = "en")
        {
            var result = await _technicianBookingService.GetByResidentIdAndSpecialization(residentId, specialization);
            return Ok(ResponseHelper.Success(LocalizationKey.GetResidentBookingsSuccessfully, lan, result));
        }
    }
}