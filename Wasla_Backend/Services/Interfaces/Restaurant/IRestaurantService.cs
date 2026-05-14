namespace Wasla_Backend.Services.Interfaces
{
    public interface IRestaurantService
    {
        public Task CompleteProfile(CompleteRegisterRestaurantDto dto);
        public Task UpdateRestaurant(UpdateRestaurantDto dto);
        public Task<PagedResult<GetAllRestaurantsResponse>> GetAll(GetGeneralWithPaginationDto<int> paginationParams);
        public Task<GetRestaurantResponse> GetRestaurant(GetGeneralDto<string> dto);
        public Task<RestaurantCharts> GetCharts(string restaurantId);
        public Task ChangeStatus (string  restaurantId,bool available);
    }
}
