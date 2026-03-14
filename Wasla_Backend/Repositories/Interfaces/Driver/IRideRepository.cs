
namespace Wasla_Backend.Repositories.Interfaces.driver
{
    public interface IRideRepository : IGenericRepository<RideModel>
    {
         public Task<bool> IsHasActiveRide(string residentId);
        public Task<RideDetailsForDriverDto> GetrideDetailsForDriver(int rideId);
        public Task<int> UpdateRideStatusAsync(int rideId, RideStatus accepted, string driverId);
        public Task<RideDetailsForResidentDto> GetrideDetailsForResident(int rideId);
        public Task<List<UserRideDto>> GetUserRides(string residentId);
        public Task<List<DriverRideDto>> GetDriverRides(string driverId);
        public Task<DriverChartDto> GetDriverChart(string driverId);



    }
}
