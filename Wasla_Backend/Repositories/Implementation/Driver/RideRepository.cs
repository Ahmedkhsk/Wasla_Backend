

namespace Wasla_Backend.Repositories.Implementation.driver
{
    public class RideRepository : GenericRepository<RideModel>, IRideRepository
    {
        public RideRepository(Context context) : base(context)
        {
        }

        public async Task<bool> IsHasActiveRide(string residentId)
        {
            return await _context.rides.AnyAsync
              (r => r.ResidentId == residentId && (r.Status == RideStatus.InProgress
              ||r.Status == RideStatus.Accepted||r.Status==RideStatus.Pending ));
        }

        public async Task<RideDetailsForDriverDto> GetrideDetailsForDriver(int rideId)
        {
            return await _context.rides.Where(r => r.Id == rideId).Include(r => r.Resident).AsNoTracking()
                .Select(r => new RideDetailsForDriverDto
                {
                    ResidentName = r.Resident.FullName,
                    ResidentPhone = r.Resident.PhoneNumber,
                    ResidentImage=FileSetting.GetMediaUrl(r.Resident.ProfilePhoto, MediaType.userImage),
                    PickUpPlace = r.PickUpPlace,
                    DropOffPlace = r.DropOffPlace,
                    Price = r.Price,
                    Distance = r.Distance
                }).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateRideStatusAsync(int rideId, RideStatus accepted, string driverId)
        {
            return await _context.rides.Where(r => r.Id == rideId&&r.Status==RideStatus.Pending)
                .ExecuteUpdateAsync(s =>
                 s.SetProperty(r => r.Status, accepted)
                .SetProperty(r => r.DriverId, driverId));
        }

        public async Task<RideDetailsForResidentDto> GetrideDetailsForResident(int rideId)
        {
            return await _context.rides.Where(r => r.Id == rideId).Include(r => r.Driver).AsNoTracking()
                .Select(r => new RideDetailsForResidentDto
                {
                    DriverName = r.Driver.FullName,
                    YearsOfExperience = r.Driver.DrivingExperienceYears,
                    Rating = r.Driver.Rating,
                    VehicleModel = r.Driver.VehicleModel,
                    VehicleNumber = r.Driver.VehicleNumber,
                    VehicleImage = FileSetting.GetMediaUrl(r.Driver.images.FirstOrDefault(), MediaType.DriverCarImage),
                    VehicleColor = r.Driver.VehicleColor.ToString(),
                    DriverPhone = r.Driver.PhoneNumber,
                    DriverImage = FileSetting.GetMediaUrl(r.Driver.ProfilePhoto, MediaType.userImage),
                    PickUpPlace = r.PickUpPlace,
                    DropOffPlace = r.DropOffPlace,
                    Price = r.Price,
                    startRide = r.RideDate,

                }).FirstOrDefaultAsync();
        }
    }
}
