namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [Authorize(Roles = "resident")]
        [HttpPost("Reservation")]
        public async Task<IActionResult> AddReservation(AddReservationDto dto, [FromQuery] LanDto lanDto)
        {
            await _reservationService.AddReservatio(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.ReservationCreatedSuccessfully, lanDto.lan));
        }

        [Authorize(Roles = "restaurant,resident")]
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus([FromQuery] ChangeStatusOfReservationDto dto)
        {
            await _reservationService.ChangeStatus(dto.reservationId, dto.status);

            return Ok(ResponseHelper.Success(LocalizationKey.ReservationStatusChangedSuccessfully, dto.lan));
        }

        [Authorize(Roles = "restaurant")]
        [HttpGet("RestaurantReservations")]
        public async Task<IActionResult> GetRestaurantReservations([FromQuery] GetGeneralWithPaginationDto<string> dto)
        {
            var reservations = await _reservationService.GetRestaurantReservations(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.ReservationRetrievedSuccessfully,
                                             dto.lan,
                                             reservations));
        }

        [Authorize(Roles = "resident")]
        [HttpGet("ResidentReservations")]
        public async Task<IActionResult> GetResidentReservations([FromQuery] GetGeneralWithPaginationDto<string> dto)
        {
            var reservations = await _reservationService.GetResidentReservations(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.ReservationRetrievedSuccessfully,
                                             dto.lan,
                                             reservations));
        }
    }
}