


using Wasla_Backend.Repositories.Interfaces.Driver;

namespace Wasla_Backend.Repositories.Implementation.Drivers
{
    public class DriverRepository:GenericRepository<DriverModel>, IDriverRepository
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

        public async Task<DriverModel> GetDriverByGmailAsync(string Gmail)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.Email == Gmail);
        }

        public async Task<DriverProfileDTO> GetDriverProfileByIdAsync(string id)
        {
            return await _context.Drivers
                .Where(d => d.Id == id).AsNoTracking()
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
                    status = d.DriverStatus.ToString(),
                    ReviewsCount = _context.Review.Count(r => r.ServiceProviderId == id),
                    NumberOfPassengers=_context.rides.Where(r=>r.DriverId==id&&r.Status==RideStatus.Completed).Select(r=>r.Id).Distinct().Count()


                }).AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public Task<bool> IsExistByVehicleNumberAsync(string vehicleNumber)
        {
            return _context.Drivers.AnyAsync(d => d.VehicleNumber == vehicleNumber);
        }
    }
}
