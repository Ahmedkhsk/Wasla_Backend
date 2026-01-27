namespace Wasla_Backend.Services.Interfaces
{
    public interface IDoctorServiceService
    {
        public Task AddServiceAsync(ServiceDto addServiceDto);
        public Task<IEnumerable<ServiceResponse>> GetServices(string docotorId, string lan);
        public Task UpdateServiceAsync(UpdateServiceDto updateServiceDto);
        public Task DeleteServiceAsync(int serviceId);
    }
}
