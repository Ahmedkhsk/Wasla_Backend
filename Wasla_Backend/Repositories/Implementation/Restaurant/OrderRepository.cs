namespace Wasla_Backend.Repositories.Implementation
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(Context context) : base(context)
        {
        }

        public async Task<PagedResult<Order>> OrdersRestaurent(GetGeneralWithPaginationDto<string> dto)
        {
            var query = _dbSet
                .Where(r => r.restaurantId == dto.id)
                .Include(r => r.resident)
                .Include(r => r.items)
                    .ThenInclude(i => i.menuItem)
                .OrderByDescending(r => r.id)
                .AsNoTracking();

            return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
        }

        public async Task<PagedResult<Order>> OrdersResident(GetGeneralWithPaginationDto<string> dto)
        {
            var query = _dbSet
                .Where(r => r.residentId == dto.id)
                .Include(r => r.restaurant)
                .Include(r => r.items)
                    .ThenInclude(i => i.menuItem)
                .OrderByDescending(r => r.id)
                .AsNoTracking();

            return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
        }
    }
}
