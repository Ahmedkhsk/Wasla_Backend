
namespace Wasla_Backend.Repositories.Interfaces.driver
{
    public interface IRideRepository : IGenericRepository<Ride>
    {
         public Task<bool> IsHasActiveRide(string residentId);
        public Task<RideDetailsDto> rideDetails(int rideId);
        public Task<int> UpdateRideStatusAsync(int rideId, RideStatus accepted, string driverId);
    }
}
