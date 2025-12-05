namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("BookService")]
        public async Task<IActionResult> BookService([FromForm] BookServiceDto bookServiceDto, string lan = "en")
        {
                await _bookService.Book(bookServiceDto);
                return Ok(ResponseHelper.Success("ServiceBookedSuccessfully", lan));
        }

        [HttpPut("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingDto updateBookingDto, [FromQuery] string lan = "en")
        {
            await _bookService.UpdateBookingStatus(updateBookingDto);
            return Ok(ResponseHelper.Success("BookingUpdatedSuccessfully", lan));
        }

        [HttpPut("UpdateBookingStatus")]
        public async Task<IActionResult> UpdateBookingStatus([FromQuery] int bookingId, [FromQuery] BookingStatus status, [FromQuery] string lan = "en")
        {
            await _bookService.UpdateBookingStatus(bookingId, status);
            return Ok(ResponseHelper.Success("BookingStatusUpdatedSuccessfully", lan));
        }

        [HttpGet("GetBookingDetailsForUser")]
        public async Task<IActionResult> GetBookingDetailsForUser([FromQuery]string userId, [FromQuery] string language="en")
        {
            var bookingDetails = await _bookService.GetBookingDetailsForUserAsync(userId, language);
            return Ok(ResponseHelper.Success("BookingRetrievedsuccess", language, bookingDetails));
        }
    }
}
