namespace Wasla_Backend.Services.Interfaces
{
    public interface IRestaurantCategoryService
    {
        public Task AddCategory(AddResturentCategoryDto dto);
        public Task UpdateCategory(UpdateResturentCategoryDto dto);
        public Task DeleteCategory(int id);
        public Task<IEnumerable<GetRestaurantCategoriesResponse>> GetAll(string lan);
    }
}
