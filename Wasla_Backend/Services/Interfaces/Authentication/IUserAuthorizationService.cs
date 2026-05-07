namespace Wasla_Backend.Services.Interfaces
{
    public interface IUserAuthorizationService
    {
        public Task CheckOwnershipByIdAsync(string ownerId);
        public Task CheckOwnershipByEmailAsync(string email);
        public Task CheckChatAccessAsync(string firstUserId, string secondUserId);
    }
}
