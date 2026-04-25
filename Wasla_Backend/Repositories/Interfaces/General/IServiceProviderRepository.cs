
namespace Wasla_Backend.Repositories.Interfaces.General
{
    public interface IServiceProviderRepository : IGenericRepository<ServiceProvider>
    {
        public Task<List<ServiceProviderInfoDto>> GetAll();
        public Task<List<ServiceProviderInfoDto>> Search(string query);


    }
}
