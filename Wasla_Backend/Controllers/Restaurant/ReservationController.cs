namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("Reservation")]
        public async Task<IActionResult> AddReservation(AddReservationDto dto)
        {
            await _reservationService.AddReservatio(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.ReservationCreatedSuccessfully, dto.lan));
        }

        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] ChangeStatusOfReservationDto dto)
        {
            await _reservationService.ChangeStatus(dto.reservationId, dto.status);
            return Ok(ResponseHelper.Success(LocalizationKey.ReservationStatusChangedSuccessfully, dto.lan));
        }

        [HttpGet("RestaurantReservations")]
        public async Task<IActionResult> GetRestaurantReservations([FromQuery] GetGeneralWithPaginationDto<string> dto)
        {
            var reservations = await _reservationService.GetRestaurantReservations(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.ReservationRetrievedSuccessfully, dto.lan, reservations));
        }

    }
}
