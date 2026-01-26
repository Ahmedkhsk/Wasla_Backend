namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        public Task<List<ServiceBookingDetailsDto>> GetBookingDetailsForUserAsync(string userId, string language);
        public Task<Booking> GetBookingByServiceDayIdAsync(int serviceDayId);
        public Task<int>GetNumberOfPatientByDoctorId(string doctorId);
        public Task<List<CollectedPerYearDto>> GetCollectedPriceBookingsPerYear();
        public Task<int> CountBookings(BookingStatus status);
        public Task<Booking> GetByIdWithIncludeAsync(int id);
        public Task<int> CountPatients(string doctorId);
        public Task<int> CountBookings(string doctorId);
        public Task<int> CountCompletedBookings(string doctorId);
        public Task<decimal> GetTotalAmount(string doctorId);
        public Task<List<CollectedPerYearDto>> GetCollectedPriceByYear(string doctorId);
        public Task<Booking>GetByServiceDayId(int serviceDayId);
        public Task<List<Booking>> GetByServiceProviderId(string userId);
        public Task<List<GetAllBookingResponse>> GetBookingsByDoctorIdAsync(string doctorId, BookingStatus status, string lan);
        public Task<bool>GetByUserIdAndDoctorID(string userId, string doctorId);
        public Task<List<Booking>> GetBookingsForResidentAsync(string residentId);
        public Task<int>CountBookingBYUserAndServiceProvider(string userId, string serviceProviderId);
        public Task<bool> HasBookingSameDay(string userId, string ServiceProviderId, DateOnly date);

    }
}
