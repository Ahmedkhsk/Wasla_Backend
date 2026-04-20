namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantMenuController : ControllerBase
    {
        private readonly IMenuItemCategoryService _menuItemCategoryService;

        public RestaurantMenuController(IMenuItemCategoryService menuItemCategoryService)
        {
            _menuItemCategoryService = menuItemCategoryService;
        }

        [HttpPost("Category")]
        public async Task<IActionResult> AddCategory(AddMenuItemCategoryDto dto , [FromQuery] LanDto lanDto)
        {
            await _menuItemCategoryService.AddCategory(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.MenuItemCategoryCreatedSuccessfully, lanDto.lan));
        }

        [HttpPut("Category")]
        public async Task<IActionResult> UpdateCategory(UpdateMenuItemCategoryDto dto, [FromQuery] LanDto lanDto)
        {
            await _menuItemCategoryService.UpdateCategory(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.MenuItemCategoryUpdatedSuccessfully, lanDto.lan));
        }

        [HttpDelete("Category")]
        public async Task<IActionResult> DeleteCategory([FromQuery] GetGeneralDto<int> dto)
        {
            await _menuItemCategoryService.DeleteCategory(dto.id);
            return Ok(ResponseHelper.Success(LocalizationKey.MenuItemCategoryDeletedSuccessfully, dto.lan));
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetMenuItemCategories([FromQuery] GetGeneralDto<string> dto)
        {
            var categories = await _menuItemCategoryService.GetMenuItemCategoryDtos(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.MenuItemCategoriesRetrievedSuccessfully, dto.lan, categories));
        }

    }
}
