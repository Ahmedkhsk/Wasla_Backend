
namespace Wasla_Backend.Services.Implementation.Driver
{
    public class RideService : IRideServices
    {
        private readonly IRideRepository _rideRepository;
        public RideService(IRideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }
        public RideEstimateDto EstimateRideAsync(CalculateRideDto calculateRideDto)
        {
            const double RoadFactor = 1.3;
            const double BaseFare = 5;
            const double Commission = 0.20;

            var distance = GeoHelper.CalculateDistance(
                calculateRideDto.PickupLatitude,
                calculateRideDto.PickupLongitude,
                calculateRideDto.DropoffLatitude,
                calculateRideDto.DropoffLongitude
            );

            distance *= RoadFactor;

            double pricePerKm = 0;

            switch (calculateRideDto.VehicleType)
            {
                case VehicleType.Scooter:
                    pricePerKm = 3;
                    break;

                case VehicleType.Car:
                    pricePerKm = 5;
                    break;

                default:
                    throw new BadRequestException(LocalizationKey.VehicleTypeNotSupported);
            }

            var ridePrice = BaseFare + (distance * pricePerKm);

            var commission = ridePrice * Commission;

            var finalPrice = ridePrice + commission;

            var rideEstimate = new RideEstimateDto
            {
                EstimatedPrice = Math.Round(finalPrice, 2),
                Distance = Math.Round(distance, 2)
            };

            return rideEstimate;
        }

    
    }
}
