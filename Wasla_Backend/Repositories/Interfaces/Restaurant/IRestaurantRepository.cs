namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IRestaurantRepository: IGenericRepository<Restaurant>
    {
        public Task<PagedResult<Restaurant>> GetAllRestaurants(PaginationParams dto);

    }
}
