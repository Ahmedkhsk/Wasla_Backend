namespace Wasla_Backend.Repositories.Implementation
{
    public class MenuItemCategoryRepository : GenericRepository<MenuItemCategory>, IMenuItemCategoryRepository
    {
        public MenuItemCategoryRepository(Context context) : base(context)
        {
        }

        public async Task<List<GetMenuItemCategoryDto>> GetMenuItemCategory(GetGeneralDto<string> dto)
        {
            var categories = await _dbSet.Where(_dbSet => _dbSet.restaurantId == dto.id).ToListAsync();
            
            return categories.Select(c => new GetMenuItemCategoryDto
            {
                id = c.id,
                name = c.name,
                nameValue = c.name.GetText(dto.lan),
                restaurantId = c.restaurantId
            }).ToList();
        }
    }
}
