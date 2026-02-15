using Wasla_Backend.Models;

namespace Wasla_Backend.Controllers.Gym
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymBookingController : ControllerBase
    {
        private readonly IGymBookingService _gymBookingService;
        private readonly IHubContext<BookingHub> _hub;

        public GymBookingController(IGymBookingService gymBookingService, IHubContext<BookingHub> hub)
        {
            _gymBookingService = gymBookingService;
            _hub = hub;
        }

        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] GymBookDto gymBookDto, string lan = "en")
        {
            var data = await _gymBookingService.Book(gymBookDto, lan);
            await _hub.Clients.All.SendAsync("PackageBooked", data);
            return Ok(ResponseHelper.Success("BookingAddedSuccessfully", lan, data.qrCodeUrl));
        }

        [HttpPut("cancel/{bookingId}")]
        public async Task<IActionResult> Cancel(int bookingId, string lan = "en")
        {
            var data = await _gymBookingService.Cancel(bookingId);
            await _hub.Clients.All.SendAsync("BookingCancelled", data);
            return Ok(ResponseHelper.Success("BookingCancelledSuccessfully", lan, data));
        }

        [HttpGet("gym/{gymId}")]
        public async Task<IActionResult> GetBookingsOfGym(string gymId, string lan = "en")
        {
            var data = await _gymBookingService.PackageBookingOFGym(gymId);
            return Ok(ResponseHelper.Success("BookingsRetrievedSuccessfully", lan, data));
        }

        [HttpGet("gym/{gymId}/status/{status}")]
        public async Task<IActionResult> GetBookingsOfGymByStatus(string gymId, GymBookingStatus status, string lan = "en")
        {
            var data = await _gymBookingService.PackagebookingOfGymAndStatus(gymId, status);
            return Ok(ResponseHelper.Success("BookingsRetrievedSuccessfully", lan, data));
        }

        [HttpGet("resident/{residentId}")]
        public async Task<IActionResult> GetBookingsOfResident(string residentId, string lan = "en")
        {
            var data = await _gymBookingService.PackagebookingOfResident(residentId);
            return Ok(ResponseHelper.Success("BookingsRetrievedSuccessfully", lan, data));
        }

        [HttpGet("resident/{residentId}/status/{status}")]
        public async Task<IActionResult> GetBookingsOfResidentByStatus(string residentId, GymBookingStatus status, string lan = "en")
        {
            var data = await _gymBookingService.PackagebookingOfResidentAndStatus(residentId, status);
            return Ok(ResponseHelper.Success("BookingsRetrievedSuccessfully", lan, data));
        }

        [HttpGet("Charts/{gymId}")]
        public async Task<IActionResult> GymCharts(string gymId, string lan = "en")
        {
            var charts = await _gymBookingService.chartsResponse(gymId);
            return Ok(ResponseHelper.Success("FetchChartSuccess", lan, charts));
        }

        [HttpGet("GetMembers/{serviceId}")]
        public async Task<IActionResult> GetMembeers(int serviceId, string lan = "en")
        {
            var data = await _gymBookingService.UserPackageResponses(serviceId);
            return Ok(ResponseHelper.Success("FetchMembersSuccess", lan, data));
        }

        [HttpGet("ValidateQr/{bookingId}")]
        public async Task<IActionResult> ValidateQr(int bookingId, string lan = "en")
        {
            var result = await _gymBookingService.ValidateQrAsync(bookingId);
            
            if (result.IsValid)
            {
                return Ok(ResponseHelper.Success("QrCodeValid", lan, result));
            }
            else
            {
                return BadRequest(ResponseHelper.Fail("QrCodeInvalid", lan));
            }
        }
    }
}
