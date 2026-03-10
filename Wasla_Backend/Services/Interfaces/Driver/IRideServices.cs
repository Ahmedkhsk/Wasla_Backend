
namespace Wasla_Backend.Services.Interfaces.Driver
{
    public interface IRideServices
    {
        public RideEstimateDto EstimateRideAsync(CalculateRideDto calculateRideDto);
    }
}
