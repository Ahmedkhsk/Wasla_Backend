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
        private readonly IHubContext<RideHub> _hub;
        private readonly Context _context;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public RideService(
            IRideRepository rideRepository,
            IResidentRepository residentRepository,
            IMapper mapper,
            DateTimeHelper dateTimeHelper,
            IDriverService driverService,
            IDriverRepository driverRepository,
            IHubContext<RideHub> hub,
            Context context,
            IFileUrlBuilderService fileUrlBuilderService
        )
        {
            _rideRepository = rideRepository;
            _residentRepository = residentRepository;
            _mapper = mapper;
            _dateTimeHelper = dateTimeHelper;
            _driverService = driverService;
            _driverRepository = driverRepository;
            _hub = hub;
            _context = context;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<int> AcceptRide(int rideId, string driverId, string lan)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);

            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);

            var affectedRows = await _rideRepository.UpdateRideStatusAsync(rideId, RideStatus.Accepted, driverId);
            if (affectedRows == 0)
                throw new BadRequestException(LocalizationKey.SomeOneHadAcceptIt);
            driver.DriverStatus = DriverStatus.OnTrip;
             _driverRepository.Update(driver);
            await _driverRepository.SaveChangesAsync();


            var metadata = new Dictionary<string, string>
            {
                { "DriverName", driver.FullName }
            };

            BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    ride.ResidentId,
                    NotificationType.rideAccepted,
                    ride.Id.ToString(),
                    _fileUrlBuilderService.GetMediaUrl(driver.ProfilePhoto, MediaType.userImage),
                    lan,
                    metadata
                ));

            await _hub.Clients.User(ride.ResidentId).SendAsync("RideAccepted", ride.Id);

            return ride.Id;
        }

        public async Task<int> CancelRide(int rideId, bool IsResident, string lan)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);

            if (ride.DriverId != null && ride.Driver == null)
                await _context.Entry(ride).Reference(r => r.Driver).LoadAsync();

            if (ride.Resident == null)
                await _context.Entry(ride).Reference(r => r.Resident).LoadAsync();

            ride.Status = RideStatus.Cancelled;

            if (ride.Driver != null)
                ride.Driver.DriverStatus = DriverStatus.Online;

            await _driverRepository.SaveChangesAsync();

            var referenceId = IsResident ? ride.DriverId : ride.ResidentId;
            if (referenceId == null)
                return ride.Id;

            var userName = IsResident ? ride.Resident?.FullName : ride.Driver?.FullName;

            var metadata = new Dictionary<string, string>
    {
        { "UserName", userName }
    };

            var image = IsResident ? ride.Resident?.ProfilePhoto : ride.Driver?.ProfilePhoto;
            var imageUrl = _fileUrlBuilderService.GetMediaUrl(image, MediaType.userImage);

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    referenceId,
                    NotificationType.rideCancelled,
                    ride.Id.ToString(),
                    imageUrl,
                    lan,
                    metadata
                ));

            return ride.Id;
        }

        public async Task<int> CompleteRide(int rideId, string lan)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);

            if (ride.Status != RideStatus.InProgress)
                throw new BadRequestException(LocalizationKey.InvalidRideStatus);

            ride.Status = RideStatus.Completed;
          

            await _context.Entry(ride).Reference(r => r.Driver).LoadAsync();
            if (ride.Driver != null)
            {
                ride.Driver.DriverStatus = DriverStatus.Online;
                ride.Driver.TripsCount += 1;
                _driverRepository.Update(ride.Driver);
            }
            await _rideRepository.SaveChangesAsync();

            var metadata = new Dictionary<string, string>
            {
                { "DriverName", ride.Driver?.FullName }
            };

            var imageUrl = _fileUrlBuilderService.GetMediaUrl(ride.Driver?.ProfilePhoto, MediaType.userImage);

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    ride.ResidentId,
                    NotificationType.rideCompleted,
                    ride.DriverId,
                    imageUrl,
                    lan,
                    metadata
                ));

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

            double pricePerKm = calculateRideDto.VehicleType switch
            {
                VehicleType.Scooter => 15,
                VehicleType.Car => 20,
                _ => throw new BadRequestException(LocalizationKey.VehicleTypeNotSupported)
            };

            var ridePrice = BaseFare + (distance * pricePerKm);
            var finalPrice = ridePrice + (ridePrice * Commission);

            return new RideEstimateDto
            {
                EstimatedPrice = Math.Round(finalPrice, 2),
                Distance = Math.Round(distance, 2)
            };
        }

        public async Task<DriverChartDto> GetDriverChart(string driverId)
        {
            var driver =await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);
           return await _rideRepository.GetDriverChart(driverId);
        }

        public async Task<List<DriverRideDto>> GetDriverRides(string driverId)
        {
            var driver= await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);
            return await _rideRepository.GetDriverRides(driverId);
        }

        public async Task<RideDetailsForDriverDto> GetrideDetailsForDriver(int rideId)
        {
            var rideDetails = await _rideRepository.GetrideDetailsForDriver(rideId);
            if (rideDetails == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);

            return rideDetails;
        }

        public async Task<RideDetailsForResidentDto> GetrideDetailsForResident(int rideId)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);

            if (ride.DriverId == null)
                throw new BadRequestException(LocalizationKey.RideNotAcceptedYet);
            if (ride.Status==RideStatus.Completed)
                throw new BadRequestException(LocalizationKey.RideCompleted);


            var rideDetails = await _rideRepository.GetrideDetailsForResident(rideId);
            rideDetails.endRide = rideDetails.startRide.AddMinutes(GeoHelper.CalculateDuration(ride.Distance));

            return rideDetails;
        }

        public async Task<List<UserRideDto>> GetUserRides(string residentId)
        {
            var resident= await _residentRepository.GetByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);
            return await _rideRepository.GetUserRides(residentId);
        }

        public async Task<int> RequestRide(RequestRideDto requestRideDto, string lan)
        {
            var resident = await _residentRepository.GetByIdAsync(requestRideDto.PassengerId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);

            var hasActiveRide = await _rideRepository.IsHasActiveRide(requestRideDto.PassengerId);
            if (hasActiveRide)
                throw new BadRequestException(LocalizationKey.ResidentHasActiveRide);

            var estimateResult = EstimateRide(new CalculateRideDto
            {
                PickupLatitude = requestRideDto.PickupLatitude,
                PickupLongitude = requestRideDto.PickupLongitude,
                DropoffLatitude = requestRideDto.DropoffLatitude,
                DropoffLongitude = requestRideDto.DropoffLongitude,
                VehicleType = requestRideDto.VehicleType
            });

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
                ServiceProviderType = ServiceProviderType.Driver,
                PickUpPlace = requestRideDto.PickUpPlace,
                DropOffPlace = requestRideDto.DropOffPlace
            };

            await _rideRepository.AddAsync(ride);
            await _rideRepository.SaveChangesAsync();

            var residentPhotoUrl = _fileUrlBuilderService.GetMediaUrl(resident.ProfilePhoto, MediaType.userImage);

            var onlineDrivers = await _driverService.GetTopNearestDriver(
                requestRideDto.PickupLatitude,
                requestRideDto.PickupLongitude,
                requestRideDto.VehicleType
            );

            foreach (var driverId in onlineDrivers)
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
                        residentPhotoUrl,
                        lan,
                        metadata
                    ));
            }

            return ride.Id;
        }

        public async Task<int> StartRide(int rideId)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
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