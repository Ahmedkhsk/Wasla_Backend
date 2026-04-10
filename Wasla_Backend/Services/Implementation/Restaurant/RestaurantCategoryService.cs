namespace Wasla_Backend.Services.Implementation
{
    public class RestaurantCategoryService : IRestaurantCategoryService
    {
        private readonly IGenericRepository<RestaurantCategory> _repo;

        public RestaurantCategoryService(IGenericRepository<RestaurantCategory> repo)
        {
            _repo = repo;
        }

        public async Task AddCategory(AddResturentCategoryDto dto)
        {
            var category = new RestaurantCategory();
            category.name = dto.name;

            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateCategory(UpdateResturentCategoryDto dto)
        {
            var category = await _repo.GetByIdAsync(dto.id);
            
            if (category == null)
               throw new NotFoundException(LocalizationKey.RestaurantCategoryNotFound);

            category.name = dto.name;

            _repo.Update(category);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _repo.GetByIdAsync(id);

            if (category == null)
                throw new NotFoundException(LocalizationKey.RestaurantCategoryNotFound);

            _repo.Delete(category);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<RestaurantCategory>> GetAll()
        {
            return await _repo.GetAllAsync();
        }

    }
}
