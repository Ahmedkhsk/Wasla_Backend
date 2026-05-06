namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantCategoryController : ControllerBase
    {
        private readonly IRestaurantCategoryService _restaurantCategoryService;

        public RestaurantCategoryController(IRestaurantCategoryService restaurantCategoryService)
        {
            _restaurantCategoryService = restaurantCategoryService;
        }

        [Authorize(Roles = "admin,superadmin")]
        [HttpPost("Category")]
        public async Task<IActionResult> AddCategory(AddResturentCategoryDto dto, [FromQuery] LanDto lanDto)
        {
            await _restaurantCategoryService.AddCategory(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantCategoryCreatedSuccessfully,
                                             lanDto.lan));
        }

        [Authorize(Roles = "admin,superadmin")]
        [HttpPut("Category")]
        public async Task<IActionResult> UpdateCategory(UpdateResturentCategoryDto dto, [FromQuery] LanDto lanDto)
        {
            await _restaurantCategoryService.UpdateCategory(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantCategoryUpdatedSuccessfully,
                                             lanDto.lan));
        }

        [Authorize(Roles = "admin,superadmin")]
        [HttpDelete("Category")]
        public async Task<IActionResult> DeleteCategory([FromQuery] GetGeneralDto<int> dto)
        {
            await _restaurantCategoryService.DeleteCategory(dto.id);

            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantCategoryDeletedSuccessfully,
                                             dto.lan));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] LanDto lanDto)
        {
            var categories = await _restaurantCategoryService.GetAll(lanDto.lan);

            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantCategoriesRetrievedSuccessfully,
                                             lanDto.lan,
                                             categories));
        }
    }
}