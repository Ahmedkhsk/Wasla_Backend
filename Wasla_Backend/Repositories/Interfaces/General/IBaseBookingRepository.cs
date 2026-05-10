namespace Wasla_Backend.Repositories.Interfaces.General
{
    public interface IBaseBookingRepository : IGenericRepository<BaseBooking>
    {
        public Task<int> CountBookings(BaseBookingStatus status);
        public Task<List<CollectedPerYearDto>> GetCollectedPriceBookingsPerYear();

        public Task<List<BookingData>> GetByResidentId(string residentId);
        
    }
}
