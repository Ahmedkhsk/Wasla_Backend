



namespace Wasla_Backend.Repositories.Implementation.driver
{
    public class DriverRepository:GenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(Context context) : base(context)
        {
        }

        public async Task<int> ChangeStatus(string driverId, DriverStatus newStatus)
        {
            return await _context.Drivers.Where(d => d.Id == driverId)
                .ExecuteUpdateAsync(s => s.SetProperty(d => d.DriverStatus, newStatus));
        }

        public async Task<List<string>> GetAllOnlineDriversIds(VehicleType vehicleType)
        {
            return await _context.Drivers.Where(d => d.DriverStatus == DriverStatus.Online&&d.VehicleType==vehicleType)
                .AsNoTracking()
                .Select(d => d.Id)
                .ToListAsync();
        }

        public async Task<Driver> GetDriverByGmailAsync(string Gmail)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.Email == Gmail);
        }

        public async Task<DriverProfileDTO> GetDriverProfileByIdAsync(string id)
        {
            return await _context.Drivers
                .Where(d => d.Id == id)
                .Select(d => new DriverProfileDTO
                {
                   
                    email = d.Email,
                    fullName = d.FullName,
                    phone = d.Phone,
                    vehicleNumber = d.VehicleNumber,
                    profilePhoto = d.ProfilePhoto,
                    drivingExperienceYears = d.DrivingExperienceYears,
                    vehicleType = (VehicleType)d.VehicleType,
                    rate = d.Rating,
                    tripsCount = d.TripsCount,
                    latitude = d.Latitude,
                    longitude = d.Longitude,
                    description = d.Description,
                    birthDay=d.BirthDay,
                    carImages=d.images,
                    driverFiles = d.DriverFiles,
                    status = (int)d.DriverStatus


                }).AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public Task<bool> IsExistByVehicleNumberAsync(string vehicleNumber)
        {
            return _context.Drivers.AnyAsync(d => d.VehicleNumber == vehicleNumber);
        }
    }
}
