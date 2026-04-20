namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem>
    {
        public Task<PagedResult<MenuItem>> GetMenuItemsByRestaurantIdAsync(GetGeneralWithPaginationDto<string> dto);
    }
}
