namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IUserEventRepository: IGenericRepository<UserEvent>
    {
        Task<List<ServiceProviderEventResponse>> GetTopServiceProvidersAsync(string userId, int top);

        Task<int> CountEventsForProviderAsync(string serviceProviderId);

        Task<List<UserEvent>> GetUserEventsAsync(string userId);
    }
}
