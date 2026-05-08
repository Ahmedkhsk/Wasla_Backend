
namespace Wasla_Backend.Repositories.Implementation.General
{
    public class BaseBookingRepository : GenericRepository<BaseBooking>, IBaseBookingRepository
    {
        public BaseBookingRepository(Context context) : base(context)
        {
        }

        public async Task<int> CountBookings(BaseBookingStatus status)
        {
            return await _context.BaseBookings
                .Where(b => b.baseBookingStatus == status)
                .CountAsync();
        }

        public async Task<List<CollectedPerYearDto>> GetCollectedPriceBookingsPerYear()
        {
            return await _context.BaseBookings
                .Where(b => b.baseBookingStatus == BaseBookingStatus.Done)
                .GroupBy(b => b.Date.Year)
                .Select(yearGroup => new CollectedPerYearDto
                {
                    year = yearGroup.Key,
                    months = yearGroup
                        .GroupBy(b => b.Date.Month)
                        .Select(monthGroup => new CollectedPerMonthDto
                        {
                            month = monthGroup.Key,
                            amount = monthGroup.Sum(b => b.price)
                        })
                        .OrderBy(m => m.month)
                        .ToList()
                })
                .OrderBy(y => y.year)
                .ToListAsync();
        }

        public async Task<List<BookingData>> GetByResidentId(string residentId)
        {
            return await _context.BaseBookings.Where(b => b.ResidentId == residentId&&b.baseBookingStatus == BaseBookingStatus.Done)
                .Select(b => new BookingData
                {
              
                    Date = b.Date,
                    Price = b.price,
                  
                })
                .ToListAsync();
        }
    }
}
