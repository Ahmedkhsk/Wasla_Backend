namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IRestaurantRepository: IGenericRepository<Restaurant>
    {
        public Task<PagedResult<Restaurant>> GetAllRestaurants(PaginationParams dto);
        public Task<Restaurant> GetByEmailAsync(string email);
        public Task<Restaurant> GetByUserIdAsync(string userId);
    }
}
