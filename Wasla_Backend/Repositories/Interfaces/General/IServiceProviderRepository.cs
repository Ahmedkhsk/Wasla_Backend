
namespace Wasla_Backend.Repositories.Interfaces.General
{
    public interface IServiceProviderRepository
    {
        Task<PagedResult<ServiceProviderInfoDto>> GetAll(int pageNumber, int pageSize);

        Task<PagedResult<ServiceProviderInfoDto>> Search(string query, int pageNumber, int pageSize);
    }
}
