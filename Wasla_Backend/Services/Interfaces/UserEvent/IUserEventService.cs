namespace Wasla_Backend.Services.Interfaces
{
    public interface IUserEventService
    {
        public Task CreateUserEventAsync(UserEventDto userEventDto);
    }
}
