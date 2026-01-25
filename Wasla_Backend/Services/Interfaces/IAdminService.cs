namespace Wasla_Backend.Services.Implementation
{
    public interface IAdminService
    {
        public Task<AdminChartResponse> GetCollectedCountBookingsPerYear(BookingStatus status);
        public Task ChangeUserStatus(ChangeUserStsatusDto changeUserStsatus);
    }
}
