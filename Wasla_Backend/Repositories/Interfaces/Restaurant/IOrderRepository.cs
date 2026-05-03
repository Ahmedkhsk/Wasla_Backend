namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<PagedResult<Order>> OrdersResident(GetGeneralWithPaginationDto<string> dto);
        public Task<PagedResult<Order>> OrdersRestaurent(GetGeneralWithPaginationDto<string> dto);
        public Task<Order?> GetOrderWithIncludeUsers(int orderId);
        public Task<Order?> GetOrderDetails(int orderId);
        public Task<int> CountOrders(string restaurantId, OrderStatus? status);
        public Task<List<CollectedPerYearDto>> GetCollectedPriceByYear(string restaurantId);
        public Task<double> TotalAmountOfOrders(string restaurantId);
        public Task<List<BookingData>> GetBookingPerUser(string residentId);
    }
}
