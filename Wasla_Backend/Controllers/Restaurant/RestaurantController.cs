namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost("CompleteProfile")]
        public async Task<IActionResult> CompleteProfile([FromForm] CompleteRegisterRestaurantDto dto)
        {
            await _restaurantService.CompleteProfile(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.ProfileCompletedSuccessfully, dto.lan));
        }

        [HttpPut("UpdateRestaurant")]
        public async Task<IActionResult> UpdateRestaurant([FromForm] UpdateRestaurantDto dto)
        {
            await _restaurantService.UpdateRestaurant(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantUpdatedSuccessfully, dto.lan));
        }

        [HttpGet("Restaurants")]
        public async Task<IActionResult> GetRestaurants([FromQuery] GetGeneralWithPaginationDto<string> dto)
        {
            var restaurants = await _restaurantService.GetAll(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantsRetrievedSuccessfully, dto.lan, restaurants));
        }
    }
}
