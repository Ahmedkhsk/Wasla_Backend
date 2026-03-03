

namespace Wasla_Backend.Repositories.Implementation.driver
{
    public class DriverRepository:GenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(Context context) : base(context)
        {
        }

        public async Task<Driver> GetDriverByGmailAsync(string Gmail)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.Email == Gmail);
        }

        public Task<bool> IsExistByVehicleNumberOrLicenseNumberAsync(string vehicleNumber, string licenseNumber)
        {
            return _context.Drivers.AnyAsync(d => d.VehicleNumber == vehicleNumber || d.LicenseNumber == licenseNumber);
        }
    }
}
