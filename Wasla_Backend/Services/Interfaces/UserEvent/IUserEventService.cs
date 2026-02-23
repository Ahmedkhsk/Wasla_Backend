namespace Wasla_Backend.Services.Interfaces
{
    public interface IUserEventService
    {
        public Task CreateUserEventAsync(UserEventDto userEventDto);
        public Task<List<ServiceProviderEventResponse>> GetMostUsedServicesGloballyAsync(int top);
        public Task<ServiceProviderRsponse> ServiceProviderRsponse(int top);
        public Task<List<ServiceProviderEventResponse>> GetTopServiceProvidersAsync(string userId, int top);
    }
}
