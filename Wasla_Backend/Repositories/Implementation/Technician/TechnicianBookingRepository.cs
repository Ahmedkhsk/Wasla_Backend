namespace Wasla_Backend.Repositories.Implementation.technician
{
    public class TechnicianBookingRepository : GenericRepository<TechnicianBooking>, ITechnicianBookingRepository
    {
        public TechnicianBookingRepository(Context context) : base(context)
        {
        }
    }
}
