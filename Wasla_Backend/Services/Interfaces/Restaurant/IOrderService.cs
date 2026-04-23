namespace Wasla_Backend.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<CheckoutResponse> Checkout(CheckoutDto dto);
        public Task<PagedResult<OrderRestaurantResponse>> OrdersRestaurant(GetGeneralWithPaginationDto<string> dto);
        public Task<PagedResult<OrderResidentResponse>> OrdersResident(GetGeneralWithPaginationDto<string> dto);
    }
}
