namespace Wasla_Backend.Services.Interfaces
{
    public interface IRestaurantService
    {
        public Task CompleteProfile(CompleteRegisterRestaurantDto dto);
        public Task UpdateRestaurant(UpdateRestaurantDto dto);
        public Task<PagedResult<GetAllRestaurantsResponse>> GetAll(PaginationParams paginationParams);
        public Task<GetRestaurantResponse> GetRestaurant(GetGeneralDto<string> dto);
    }
}
