namespace Wasla_Backend.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task AddItem(AddMenuItemDto dto);

        Task UpdateItem(UpdateMenuItemDto dto);

        Task DeleteItem(int id);
        Task<PagedResult<GetMenuItemDto>> GetMenuItemsByRestaurantIdAsync(GetGeneralWithPaginationDto<string> dto);
        Task<List<GetItemsbyCategoryResponse>> GetMenuItemsByCategoryAsync(GetGeneralDto<string> dto);
    }
}
