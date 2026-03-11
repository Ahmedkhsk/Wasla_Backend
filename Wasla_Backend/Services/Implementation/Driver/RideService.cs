

namespace Wasla_Backend.Services.Implementation.Driver
{
    public class RideService : IRideServices
    {
        private readonly IRideRepository _rideRepository;
        private readonly IResidentRepository _residentRepository;
        private readonly IMapper _mapper;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IDriverService _driverService;
        private readonly IDriverRepository _driverRepository;

        public RideService(IRideRepository rideRepository, IResidentRepository residentRepository,
            IMapper mapper,DateTimeHelper dateTimeHelper, IDriverService driverService, IDriverRepository driverRepository)
        {
            _rideRepository = rideRepository;
            _residentRepository = residentRepository;
            _mapper = mapper;
            _dateTimeHelper = dateTimeHelper;
            _driverService = driverService;
            _driverRepository = driverRepository;
        }

        public async Task<int> AcceptRide(int rideId, string driverId)
        {
            var ride =await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);
            var driver =await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);
             var affectedRows = await _rideRepository.UpdateRideStatusAsync(rideId, RideStatus.Accepted, driverId);
            if (affectedRows == 0)
                throw new BadRequestException(LocalizationKey.SomeOneHadAcceptIt);
            var metadata = new Dictionary<string, string>
    {
        { "DriverName", driver.FullName }
    };

            BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    ride.ResidentId,
                    NotificationType.rideAccepted,
                    ride.Id.ToString(),
                    driver.ProfilePhoto,
                    "en",
                    metadata
                ));

            return ride.Id;
        }

        public async Task<int> CancelRide(int rideId)
        {
            var ride =await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);
          
            ride.Status = RideStatus.Cancelled;
            _rideRepository.Update(ride);
            await _rideRepository.SaveChangesAsync();
            return ride.Id;
        }

        public async Task<int> CompleteRide(int rideId)
        {
            var ride =await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);
            if (ride.Status != RideStatus.InProgress)
                throw new BadRequestException(LocalizationKey.InvalidRideStatus);
            ride.Status = RideStatus.Completed;
            _rideRepository.Update(ride);
            await _rideRepository.SaveChangesAsync();
            return ride.Id;
        }

        public RideEstimateDto EstimateRide(CalculateRideDto calculateRideDto)
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
                    pricePerKm = 15;
                    break;

                case VehicleType.Car:
                    pricePerKm = 20;
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

        public async Task<RideDetailsDto> GetrideDetails(int rideId)
        {
            var rideDetails =await _rideRepository.rideDetails(rideId);
            if (rideDetails == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);
            return rideDetails;
        }

        public async Task<int> RequestRide(RequestRideDto requestRideDto)
        {
           var resident =await _residentRepository.GetByIdAsync(requestRideDto.PassengerId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);
            var hasActiveRide = await _rideRepository.IsHasActiveRide(requestRideDto.PassengerId);
            if (hasActiveRide)
                throw new BadRequestException(LocalizationKey.ResidentHasActiveRide);
            var estimate = new CalculateRideDto
            {
                PickupLatitude = requestRideDto.PickupLatitude,
                PickupLongitude = requestRideDto.PickupLongitude,
                DropoffLatitude = requestRideDto.DropoffLatitude,
                DropoffLongitude = requestRideDto.DropoffLongitude,
                VehicleType = requestRideDto.VehicleType
            };
            var estimateResult = EstimateRide(estimate);
            var ride = new RideModel
            {
                ResidentId = requestRideDto.PassengerId,
                PickupLatitude = requestRideDto.PickupLatitude,
                PickupLongitude = requestRideDto.PickupLongitude,
                DropoffLatitude = requestRideDto.DropoffLatitude,
                DropoffLongitude = requestRideDto.DropoffLongitude,
                RideDate = _dateTimeHelper.Now,
                Status = RideStatus.Pending,
                Price = estimateResult.EstimatedPrice,
                Distance = estimateResult.Distance,
                ServiceProviderType = ServiceProviderType.Driver

            };
           
            await _rideRepository.AddAsync(ride);
            await _rideRepository.SaveChangesAsync();

            var OnlineDrivers = await _driverService.GetTopNearestDriver(requestRideDto.PickupLatitude,
                requestRideDto.PickupLongitude,requestRideDto.VehicleType);
            Console.WriteLine("Online Drivers: " + string.Join(", ", OnlineDrivers));
            foreach (var driverId in OnlineDrivers)
            {
                var metadata = new Dictionary<string, string>
    {
        { "Distance", ride.Distance.ToString("0.0") },
        { "Price", ride.Price.ToString("0.0") }
    };

                Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                    x => x.sendNotification(
                        driverId,
                        NotificationType.newRideRequest,
                        ride.Id.ToString(),
                        resident.ProfilePhoto,
                        "en",
                        metadata
                    ));
            }
            return ride.Id;



        }

        public async Task<int> StartRide(int rideId)
        {
            var ride=await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);
            if (ride.Status != RideStatus.Accepted)
                throw new BadRequestException(LocalizationKey.InvalidRideStatus);
            ride.Status = RideStatus.InProgress;
            _rideRepository.Update(ride);
            await _rideRepository.SaveChangesAsync();
            return ride.Id;
        }
    }
}
