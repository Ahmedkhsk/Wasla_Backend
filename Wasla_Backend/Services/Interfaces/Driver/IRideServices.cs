
namespace Wasla_Backend.Services.Interfaces.Driver
{
    public interface IRideServices
    {
        public RideEstimateDto EstimateRide(CalculateRideDto calculateRideDto);
        public Task<int>RequestRide(RequestRideDto requestRideDto);
        public Task<RideDetailsDto> GetrideDetails(int rideId);
        public Task<int>AcceptRide(int rideId, string driverId);
        public Task<int> CompleteRide(int rideId);
        public Task<int> CancelRide(int rideId);

    }
}
