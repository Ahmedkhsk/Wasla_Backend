namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorBookController : ControllerBase
    {
        private readonly IDoctorBookService _doctorBookService;
        public DoctorBookController(IDoctorBookService doctorBookService)
        {
            _doctorBookService = doctorBookService;
        }

        [HttpPost("BookService")]
        public async Task<IActionResult> BookService([FromForm] BookServiceDto bookServiceDto, string lan = "en")
        {
                await _doctorBookService.Book(bookServiceDto);
                return Ok(ResponseHelper.Success("ServiceBookedSuccessfully", lan));
        }

        [HttpPut("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingDto updateBookingDto, [FromQuery] string lan = "en")
        {
            await _doctorBookService.UpdateBooking(updateBookingDto);
            return Ok(ResponseHelper.Success("BookingUpdatedSuccessfully", lan));
        }

        [HttpPut("UpdateBookingStatus")]
        public async Task<IActionResult> UpdateBookingStatus([FromQuery] int bookingId, [FromQuery] BookingStatus status, [FromQuery] string lan = "en")
        {
            await _doctorBookService.UpdateBookingStatus(bookingId, status);
            return Ok(ResponseHelper.Success("BookingStatusUpdatedSuccessfully", lan));
        }

        [HttpGet("GetBookingDetailsForUser")]
        public async Task<IActionResult> GetBookingDetailsForUser([FromQuery]string userId, [FromQuery] string language="en")
        {
            var bookingDetails = await _doctorBookService.GetBookingDetailsForUserAsync(userId, language);
            return Ok(ResponseHelper.Success("BookingRetrievedsuccess", language, bookingDetails));
        }
    }
}
