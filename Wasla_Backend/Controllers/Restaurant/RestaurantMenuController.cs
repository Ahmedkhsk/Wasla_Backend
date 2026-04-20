namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantMenuController : ControllerBase
    {
        private readonly IMenuItemCategoryService _menuItemCategoryService;
        private readonly IMenuItemService _menuItemService;

        public RestaurantMenuController(IMenuItemCategoryService menuItemCategoryService, IMenuItemService menuItemService)
        {
            _menuItemCategoryService = menuItemCategoryService;
            _menuItemService = menuItemService;
        }

        [HttpPost("Category")]
        public async Task<IActionResult> AddCategory(AddMenuItemCategoryDto dto, [FromQuery] LanDto lanDto)
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

        [HttpPost("Item")]
        public async Task<IActionResult> AddItem(AddMenuItemDto dto, [FromQuery] LanDto lanDto)
        {
            await _menuItemService.AddItem(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.MenuItemCreatedSuccessfully, lanDto.lan));
        }

        [HttpPut("Item")]
        public async Task<IActionResult> UpdateItem(UpdateMenuItemDto dto, [FromQuery] LanDto lanDto)
        {
            await _menuItemService.UpdateItem(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.MenuItemUpdatedSuccessfully, lanDto.lan));
        }

        [HttpDelete("Item")]
        public async Task<IActionResult> DeleteItem([FromQuery] GetGeneralDto<int> dto)
        {
            await _menuItemService.DeleteItem(dto.id);
            return Ok(ResponseHelper.Success(LocalizationKey.MenuItemDeletedSuccessfully, dto.lan));
        }

        [HttpGet("Items")]
        public async Task<IActionResult> GetMenuItemsByRestaurantId([FromQuery] GetGeneralWithPaginationDto<string> dto)
        {
            var menuItems = await _menuItemService.GetMenuItemsByRestaurantIdAsync(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.MenuItemsRetrievedSuccessfully, dto.lan, menuItems));
        }
    }
}
