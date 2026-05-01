
namespace Wasla_Backend.Repositories.Implementation.General
{
    public class BaseBookingRepository : GenericRepository<BaseBooking>, IBaseBookingRepository
    {
        public BaseBookingRepository(Context context) : base(context)
        {
        }

        public async Task<List<BookingData>> GetByResidentId(string residentId)
        {
            return await _context.BaseBookings.Where(b => b.ResidentId == residentId&&b.IsPaid)
                .Select(b => new BookingData
                {
              
                    Date = b.Date,
                    Price = b.price,
                  
                })
                .ToListAsync();
        }
    }
}
