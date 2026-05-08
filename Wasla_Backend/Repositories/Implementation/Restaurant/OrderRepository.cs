
namespace Wasla_Backend.Repositories.Implementation
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(Context context) : base(context)
        {
        }
        public async Task<Order?> GetOrderDetails(int orderId)
        {
            return await _dbSet
                .Where(r => r.id == orderId)
                .Include(r => r.items)
                    .ThenInclude(i => i.menuItem)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Order?> GetOrderWithIncludeUsers(int orderId)
        {
            return await _dbSet
                .Where(r => r.id == orderId)
                .Include(r => r.resident)
                .Include(r => r.restaurant)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CountOrders(OrderStatus? status)
        {
            var query = _dbSet.AsQueryable();
            if (status.HasValue)
                query = query.Where(o => o.status == status);
            return await query.CountAsync();
        }

        public async Task<List<CollectedPerYearDto>> GetCollectedPriceOrdersPerYear()
        {
            return await _context.Orders
                .Where(o => o.status == OrderStatus.Delivered)
                .GroupBy(o => o.createdAt.Year)
                .Select(yearGroup => new CollectedPerYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(o => o.createdAt.Month)
                        .Select(monthGroup => new CollectedPerMonthDto
                        {
                            month = monthGroup.Key,
                            amount = (double)monthGroup.Sum(o => o.totalPrice + o.deliveryFee)
                        })
                        .OrderBy(m => m.month)
                        .ToList()
                })
                .OrderBy(y => y.year)
                .ToListAsync();
        }

        public async Task<PagedResult<Order>> OrdersRestaurent(GetGeneralWithPaginationDto<string> dto)
        {
            var query = _dbSet
                .Where(r => r.restaurantId == dto.id)
                .OrderByDescending(r=>r.createdAt)
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
                .OrderByDescending(r=>r.createdAt)
                .Include(r => r.restaurant)
                .Include(r => r.items)
                    .ThenInclude(i => i.menuItem)
                .OrderByDescending(r => r.id)
                .AsNoTracking();

            return await query.ToPagedResultAsync(dto.PageNumber, dto.PageSize);
        }
        public async Task<int> CountOrders(string restaurantId, OrderStatus? status)
        {
            var query = _dbSet.Where(o => o.restaurantId == restaurantId);

            if (status.HasValue)
                query = query.Where(o => o.status == status);

            return await query.CountAsync();
        }

        public async Task<double> TotalAmountOfOrders(string restaurantId)
        {
            return await _dbSet
                .Where(o => o.restaurantId == restaurantId && o.status == OrderStatus.Delivered && o.paymentStatus == PaymentStatus.Completed)
                .SumAsync(o => (double)o.totalPrice);
        }

        public async Task<List<CollectedPerYearDto>> GetCollectedPriceByYear(string restaurantId)
        {
            return await _context.Orders
                .Where(b => b.restaurantId == restaurantId
                    && b.status == OrderStatus.Delivered&& b.paymentStatus == PaymentStatus.Completed)
                .GroupBy(b => b.createdAt.Year)
                .Select(yearGroup => new CollectedPerYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(b => b.createdAt.Month)
                        .Select(monthGroup => new CollectedPerMonthDto
                        {
                            month = monthGroup.Key,
                            amount = (double)monthGroup.Sum(b => b.totalPrice)
                        })
                        .OrderBy(m => m.month)
                        .ToList()
                })
                .OrderBy(y => y.year)
                .ToListAsync();
        }

        public async Task<List<BookingData>> GetBookingPerUser(string residentId)
        {
            return await _context.Orders.Where(o => o.residentId == residentId&&o.status==OrderStatus.Delivered)
                .Select(o => new BookingData
                {
                    Date = o.createdAt,
                    Price =(double)o.totalPrice,
                })
                .ToListAsync();
        }
    }
}
