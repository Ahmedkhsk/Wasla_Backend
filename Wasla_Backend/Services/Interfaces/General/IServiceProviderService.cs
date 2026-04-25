namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IServiceProviderService
    {
        public Task<List<ServiceProviderInfoDto>> GetAll();
        public Task<List<ServiceProviderInfoDto>> Search(string query);


    }
}
