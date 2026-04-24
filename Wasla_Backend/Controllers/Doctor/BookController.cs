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
                var id=await _doctorBookService.Book(bookServiceDto);
                return Ok(ResponseHelper.Success(LocalizationKey.ServiceBookedSuccessfully, lan,id));
        }

        [HttpPut("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingDto updateBookingDto, [FromQuery] string lan = "en")
        {
            await _doctorBookService.UpdateBooking(updateBookingDto);
            return Ok(ResponseHelper.Success(LocalizationKey.BookingUpdatedSuccessfully, lan));
        }

        [HttpPut("UpdateBookingStatus")]
        public async Task<IActionResult> UpdateBookingStatus([FromQuery] int bookingId, [FromQuery] BookingStatus status,bool isResident, [FromQuery] string lan = "en")
        {
            await _doctorBookService.UpdateBookingStatus(bookingId, status, isResident);
            return Ok(ResponseHelper.Success(LocalizationKey.BookingStatusUpdatedSuccessfully, lan));
        }

        [HttpGet("GetBookingDetailsForUser")]
        public async Task<IActionResult> GetBookingDetailsForUser([FromQuery]string userId, [FromQuery] string language="en")
        {
            var bookingDetails = await _doctorBookService.GetBookingDetailsForUserAsync(userId, language);
            return Ok(ResponseHelper.Success(LocalizationKey.BookingRetrievedsuccess, language, bookingDetails));
        }
    }
}
