
namespace Wasla_Backend.Repositories.Interfaces.Driver
{
    public interface IDriverRepository:IGenericRepository<DriverModel>
    {
        public Task<DriverModel> GetDriverByGmailAsync(string Gmail);
        public Task<bool> IsExistByVehicleNumberAsync(string vehicleNumber);
        public Task<DriverProfileDTO> GetDriverProfileByIdAsync(string id);
        public Task<int> ChangeStatus(string driverId, DriverStatus newStatus);
        public Task<List<string>> GetAllOnlineDriversIds(VehicleType vehicleType);
    }
}
