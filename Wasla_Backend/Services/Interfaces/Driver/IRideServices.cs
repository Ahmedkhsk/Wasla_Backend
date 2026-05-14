
namespace Wasla_Backend.Services.Interfaces.Driver
{
    public interface IRideServices
    {
        public RideEstimateDto EstimateRide(CalculateRideDto calculateRideDto);
        public Task<List<AllNearestDriverDto>> RequestRide(RequestRideDto requestRideDto);
        public Task<RideDetailsForDriverDto> GetrideDetailsForDriver(int rideId);
        public Task<int>AcceptRide(int rideId, string driverId,string lan);
        public Task<int> CompleteRide(int rideId,string lan);
        public Task<int> CancelRide(int rideId,bool IsResident,string lan);
        public Task<int>StartRide(int rideId);
        public Task<RideDetailsForResidentDto> GetrideDetailsForResident(int rideId);
        public Task<List<UserRideDto>> GetUserRides(string residentId);
        public Task<List<DriverRideDto>> GetDriverRides(string driverId);
        public Task<DriverChartDto> GetDriverChart(string driverId);
        public Task CheckRideAcceptance(int rideId);
        public Task<int> ChooseDriver(ChooseDriverDto chooseDriverDto, string lan);
        public Task<int> RejectRide(int rideId, string driverId, string lan);





    }
}
