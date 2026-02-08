namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IServiceHandler
    {
        public ServiceProviderType type { get; set; }
        public Task AddService();
        public Task RemoveService();
        public Task UpdateService();
    }
}
