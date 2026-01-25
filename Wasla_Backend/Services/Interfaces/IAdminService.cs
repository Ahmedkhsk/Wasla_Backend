namespace Wasla_Backend.Services.Implementation
{
    public interface IAdminService
    {
        public Task<AdminChartResponse> GetCollectedCountBookingsPerYear(BookingStatus status);


    }
}
