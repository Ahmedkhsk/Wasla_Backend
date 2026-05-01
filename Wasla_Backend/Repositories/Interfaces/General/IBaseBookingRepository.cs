namespace Wasla_Backend.Repositories.Interfaces.General
{
    public interface IBaseBookingRepository : IGenericRepository<BaseBooking>
    {
        public Task<List<BookingData>> GetByResidentId(string residentId);
    }
}
