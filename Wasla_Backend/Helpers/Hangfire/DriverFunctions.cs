namespace Wasla_Backend.Helpers.Hangfire
{
    public class DriverFunctions
    {
        private readonly IRideServices _rideServices;
        public DriverFunctions(IRideServices rideServices)
        {
            _rideServices = rideServices;
        }
        public async Task CheckRideAcceptance(int rideId)
        {
            await _rideServices.CheckRideAcceptance(rideId);
        }
    }
}
