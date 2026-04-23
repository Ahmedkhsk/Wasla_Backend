namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<PagedResult<Order>> OrdersResident(GetGeneralWithPaginationDto<string> dto);
        public Task<PagedResult<Order>> OrdersRestaurent(GetGeneralWithPaginationDto<string> dto);
    }
}
