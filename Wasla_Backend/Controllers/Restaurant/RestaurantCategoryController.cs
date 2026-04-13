namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantCategoryController : ControllerBase
    {
        private readonly IRestaurantCategoryService _restaurantCategoryService;

        public RestaurantCategoryController(IRestaurantCategoryService restaurantCategoryService)
        {
            _restaurantCategoryService = restaurantCategoryService;
        }

        [HttpPost("Category")]
        public async Task<IActionResult> AddCategory(AddResturentCategoryDto dto)
        {
            await _restaurantCategoryService.AddCategory(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantCategoryCreatedSuccessfully, dto.lan));
        }

        [HttpPut("Category")]
        public async Task<IActionResult> UpdateCategory(UpdateResturentCategoryDto dto)
        {
            await _restaurantCategoryService.UpdateCategory(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantCategoryUpdatedSuccessfully, dto.lan));
        }

        [HttpDelete("Category")]
        public async Task<IActionResult> DeleteCategory([FromQuery] GetGeneralDto<int> dto)
        {
            await _restaurantCategoryService.DeleteCategory(dto.id);
            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantCategoryDeletedSuccessfully, dto.lan));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(LanDto lanDto)
        {
            var categories = await _restaurantCategoryService.GetAll(lanDto.lan);
            return Ok(ResponseHelper.Success(LocalizationKey.RestaurantCategoriesRetrievedSuccessfully, lanDto.lan, categories));
        }
    }
}
