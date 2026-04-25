namespace Wasla_Backend.Services.Interfaces.General
{
    public interface IServiceProviderService
    {
        Task<PagedResult<ServiceProviderInfoDto>> GetAll(int pageNumber, int pageSize);

        Task<PagedResult<ServiceProviderInfoDto>> Search(string query, int pageNumber, int pageSize);


    }
}
