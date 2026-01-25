namespace Wasla_Backend.Services.Implementation
{
    public interface IAdminService
    {
        public Task<AdminChartResponse> GetCollectedCountBookingsPerYear(BookingStatus status);
        public Task ChangeUserStatus(ChangeUserStsatusDto changeUserStsatus);
        public Task AddContut(ContactUsDto contactUsDto);
        public Task<IEnumerable<ContactUs>> GetContacts();
    }
}
