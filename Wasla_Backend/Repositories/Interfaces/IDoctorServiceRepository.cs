namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IDoctorServiceRepository : IGenericRepository<Service>
    {
        Task DeleteByIdAsync(int id);
        public Task<IEnumerable<Service>> GetAllServicesAsync(string id);
        public Task<Service> GetServiceIncludeDaysAsync(int id);
        public Task<Service> GetAllServiceAsync(int id);
    }
}
