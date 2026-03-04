namespace Wasla_Backend.Repositories.Interfaces.driver
{
    public interface IDriverRepository:IGenericRepository<Driver>
    {
        public Task<Driver> GetDriverByGmailAsync(string Gmail);
        public Task<bool> IsExistByVehicleNumberOrLicenseNumberAsync(string vehicleNumber, string licenseNumber);
        public Task<DriverProfileDTO> GetDriverProfileByIdAsync(string id);
        public Task<int> ChangeStatus(string driverId, DriverStatus newStatus);
    }
}
