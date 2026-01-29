namespace Wasla_Backend.Services.Implementation
{
    public interface IAdminService
    {
        public Task<AdminChartResponse> GetCollectedCountBookingsPerYear(BookingStatus status);
        public Task ChangeUserStatus(ChangeUserStsatusDto changeUserStsatus);
        public Task AddContact(ContactUsDto contactUsDto);
        public Task<IEnumerable<ContactUs>> GetContacts();
        public Task<AdminUserDetailsResponseDto> GetUserDetailsAsync(string userId);
        public Task<PagedResult<UserApproveResponse>> UserApproveResponses(string roleId, int pageNumber, int pageSize);
    }
}
