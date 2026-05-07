namespace Wasla_Backend.Controllers.Gym
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GymBookingController : ControllerBase
    {
        private readonly IGymBookingService _gymBookingService;
        private readonly IHubContext<BookingHub> _hub;

        public GymBookingController(IGymBookingService gymBookingService, IHubContext<BookingHub> hub)
        {
            _gymBookingService = gymBookingService;
            _hub = hub;
        }

        [Authorize(Roles = "resident")]
        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] GymBookDto gymBookDto, [FromQuery] LanDto lanDto)
        {
            var data = await _gymBookingService.Book(gymBookDto, lanDto.lan);

            await _hub.Clients.All.SendAsync("PackageBooked", data);

            var responseData = new BookingResponseDto
            {
                bookingId = data.bookingId,
            };

            return Ok(ResponseHelper.Success(LocalizationKey.BookingAddedSuccessfully,
                                             lanDto.lan,
                                             responseData));
        }

        [Authorize(Roles = "resident,gym")]
        [HttpPut("cancel/{bookingId}")]
        public async Task<IActionResult> Cancel(int bookingId, bool isResident, [FromQuery] LanDto lanDto)
        {
            var data = await _gymBookingService.Cancel(bookingId, isResident);

            await _hub.Clients.All.SendAsync("BookingCancelled", data);

            return Ok(ResponseHelper.Success(LocalizationKey.BookingCancelledSuccessfully,
                                             lanDto.lan,
                                             data));
        }

        [Authorize(Roles = "gym")]
        [HttpGet("gym/{gymId}")]
        public async Task<IActionResult> GetBookingsOfGym(string gymId, [FromQuery] LanDto lanDto)
        {
            var data = await _gymBookingService.PackageBookingOFGym(gymId);

            return Ok(ResponseHelper.Success(LocalizationKey.BookingsRetrievedSuccessfully,
                                             lanDto.lan,
                                             data));
        }

        [Authorize(Roles = "gym")]
        [HttpGet("gym/{gymId}/status/{status}")]
        public async Task<IActionResult> GetBookingsOfGymByStatus(string gymId,
                                                                  GymBookingStatus status,
                                                                  [FromQuery] LanDto lanDto)
        {
            var data = await _gymBookingService.PackagebookingOfGymAndStatus(gymId, status);

            return Ok(ResponseHelper.Success(LocalizationKey.BookingsRetrievedSuccessfully,
                                             lanDto.lan,
                                             data));
        }

        [Authorize(Roles = "resident")]
        [HttpGet("resident/{residentId}")]
        public async Task<IActionResult> GetBookingsOfResident(string residentId, [FromQuery] LanDto lanDto)
        {
            var data = await _gymBookingService.PackagebookingOfResident(residentId);

            return Ok(ResponseHelper.Success(LocalizationKey.BookingsRetrievedSuccessfully,
                                             lanDto.lan,
                                             data));
        }

        [Authorize(Roles = "resident")]
        [HttpGet("resident/{residentId}/status/{status}")]
        public async Task<IActionResult> GetBookingsOfResidentByStatus(string residentId,
                                                                       GymBookingStatus status,
                                                                       [FromQuery] LanDto lanDto)
        {
            var data = await _gymBookingService.PackagebookingOfResidentAndStatus(residentId, status);

            return Ok(ResponseHelper.Success(LocalizationKey.BookingsRetrievedSuccessfully,
                                             lanDto.lan,
                                             data));
        }

        [Authorize(Roles = "gym")]
        [HttpGet("Charts/{gymId}")]
        public async Task<IActionResult> GymCharts(string gymId, [FromQuery] LanDto lanDto)
        {
            var charts = await _gymBookingService.chartsResponse(gymId);

            return Ok(ResponseHelper.Success(LocalizationKey.FetchChartSuccess,
                                             lanDto.lan,
                                             charts));
        }

        [Authorize(Roles = "gym")]
        [HttpGet("GetMembers/{serviceId}")]
        public async Task<IActionResult> GetMembeers(int serviceId, [FromQuery] LanDto lanDto)
        {
            var data = await _gymBookingService.UserPackageResponses(serviceId);

            return Ok(ResponseHelper.Success(LocalizationKey.FetchMembersSuccess,
                                             lanDto.lan,
                                             data));
        }

        [Authorize(Roles = "gym")]
        [HttpGet("ValidateQr/{bookingId}")]
        public async Task<IActionResult> ValidateQr(int bookingId, [FromQuery] LanDto lanDto)
        {
            var result = await _gymBookingService.ValidateQrAsync(bookingId);

            if (result.IsValid)
            {
                return Ok(ResponseHelper.Success(LocalizationKey.QrCodeValid,
                                                 lanDto.lan,
                                                 result));
            }

            return Ok(ResponseHelper.Success(LocalizationKey.QrCodeInvalid,
                                             lanDto.lan));
        }
    }
}