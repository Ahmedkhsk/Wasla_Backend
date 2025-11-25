namespace Wasla_Backend.Repositories.Implementation
{
    public class BookingRepository:GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(Context context) : base(context)
        {
        }

    }
}
