namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IMenuItemCategoryRepository : IGenericRepository<MenuItemCategory>
    {
        public Task<List<GetMenuItemCategoryDto>> GetMenuItemCategory(GetGeneralDto<string> dto);

    }
}
