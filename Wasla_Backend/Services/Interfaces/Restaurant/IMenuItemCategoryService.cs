namespace Wasla_Backend.Services.Interfaces
{
    public interface IMenuItemCategoryService
    {
        public Task AddCategory(AddMenuItemCategoryDto dto);

        public Task UpdateCategory(UpdateMenuItemCategoryDto dto);

        public Task DeleteCategory(int id);

        public Task<List<GetMenuItemCategoryDto>> GetMenuItemCategoryDtos(GetGeneralDto<string> dto);
    }
}
