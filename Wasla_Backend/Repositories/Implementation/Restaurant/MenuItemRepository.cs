using Wasla_Backend.Models.Restaurant;

namespace Wasla_Backend.Repositories.Implementation
{
    public class MenuItemRepository : GenericRepository<MenuItem> , IMenuItemRepository
    {
        public MenuItemRepository(Context context) : base(context)
        {
        }

        public async Task<PagedResult<MenuItem>> GetMenuItemsByRestaurantIdAsync(GetGeneralWithPaginationDto<string> dto)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(m => m.restaurantId == dto.id)
                .Include(m => m.category)
                .AsQueryable();

            return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
        }

        public async Task<List<MenuItem>> GetMenuItemsByRestaurantIdAsync(GetGeneralDto<string> dto)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(m => m.restaurantId == dto.id)
                .Include(m => m.category)
                .ToListAsync();
        }





    }
}
