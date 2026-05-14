namespace Wasla_Backend.Services.Implementation.Driver
{
    public class RideService : IRideServices
    {
        private readonly IRideRepository _rideRepository;
        private readonly IResidentRepository _residentRepository;
        private readonly IMapper _mapper;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDriverService _driverService;
        private readonly IDriverRepository _driverRepository;
        private readonly IHubContext<RideHub> _hub;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IEntityLoader _entityLoader;
        private readonly IUserAuthorizationService _userAuthorizationService;


        public RideService(
            IRideRepository rideRepository,
            IResidentRepository residentRepository,
            IMapper mapper,
            IDateTimeHelper dateTimeHelper,
            IDriverService driverService,
            IDriverRepository driverRepository,
            IHubContext<RideHub> hub,
            IFileUrlBuilderService fileUrlBuilderService,
            IEntityLoader entityLoader,
            IUserAuthorizationService userAuthorizationService
        )
        {
            _rideRepository = rideRepository;
            _residentRepository = residentRepository;
            _mapper = mapper;
            _dateTimeHelper = dateTimeHelper;
            _driverService = driverService;
            _driverRepository = driverRepository;
            _hub = hub;
            _entityLoader = entityLoader;
            _fileUrlBuilderService = fileUrlBuilderService;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task<int> AcceptRide(int rideId, string driverId, string lan)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(driverId);

            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);

            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);
            if(driver.DriverStatus== DriverStatus.OnTrip)
                throw new BadRequestException(LocalizationKey.DriverOnTrip);
            if (ride.Status != RideStatus.Pending)
                throw new BadRequestException(LocalizationKey.RideNotAvailable);

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
            var photo = _fileUrlBuilderService.GetMediaUrl(driver.ProfilePhoto, MediaType.userImage);
            BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    ride.ResidentId,
                    NotificationType.rideAccepted,
                    ride.Id.ToString(),
                    photo,
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
            ride.baseBookingStatus = BaseBookingStatus.Cancelled;
            if (IsResident)
                await _userAuthorizationService.CheckOwnershipByIdAsync(ride.ResidentId);
            else
                await _userAuthorizationService.CheckOwnershipByIdAsync(ride.DriverId);

            if (ride.Status == RideStatus.Cancelled)
                throw new BadRequestException(LocalizationKey.RideAlreadyCancelled);

            if (ride.DriverId != null && ride.Driver == null)
                await _entityLoader.LoadReferenceAsync(ride, r => r.Driver);

            if (ride.Resident == null)
                await _entityLoader.LoadReferenceAsync(ride, r => r.Resident);

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

        public async Task CheckRideAcceptance(int rideId)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null || ride.Status != RideStatus.Pending)
                return;
            ride.baseBookingStatus = BaseBookingStatus.Cancelled;
            
            await _rideRepository.UpdateRideStatusAsync(rideId, RideStatus.Rejected, null);
            await _rideRepository.SaveChangesAsync();

            var target = string.Concat(ride.Id, " , ", ride.DriverId);

            BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    ride.ResidentId,
                    NotificationType.rideRejected,
                    target,
                    null,
                    "en",
                    null
                ));

            await _hub.Clients.User(ride.ResidentId).SendAsync("RideRejected", ride.Id);
        }

        public async Task<int> CompleteRide(int rideId, string lan)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);
            await _userAuthorizationService.CheckOwnershipByIdAsync(ride.DriverId);

            if (ride.Status != RideStatus.InProgress)
                throw new BadRequestException(LocalizationKey.InvalidRideStatus);

            ride.Status = RideStatus.Completed;
            ride.baseBookingStatus = BaseBookingStatus.Done;
            ride.IsPaid=true;


            await _entityLoader.LoadReferenceAsync(ride, r => r.Driver);
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
            await _userAuthorizationService.CheckOwnershipByIdAsync(driverId);
            var driver =await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);
           return await _rideRepository.GetDriverChart(driverId);
        }

        public async Task<List<DriverRideDto>> GetDriverRides(string driverId)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(driverId);
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
            var Duration = GeoHelper.CalculateDuration(rideDetails.Distance);
            rideDetails.DropOffTime = rideDetails.PickUpTime.AddMinutes(Duration);
            rideDetails.Duration = Duration;


            return rideDetails;
        }

        public async Task<RideDetailsForResidentDto> GetrideDetailsForResident(int rideId)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);
            await _userAuthorizationService.CheckOwnershipByIdAsync(ride.ResidentId);

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
            await _userAuthorizationService.CheckOwnershipByIdAsync(residentId);
            var resident= await _residentRepository.GetByIdAsync(residentId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);
            return await _rideRepository.GetUserRides(residentId);
        }

        public async Task<List<AllNearestDriverDto>> RequestRide(RequestRideDto requestRideDto)
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

            var onlineDrivers = await _driverService.GetTopNearestDriver(
                requestRideDto.PickupLatitude,
                requestRideDto.PickupLongitude,
                requestRideDto.VehicleType
            );

            onlineDrivers.ForEach(d => d.Price = CalculatePriceByRating(estimateResult.EstimatedPrice, d.Rate));
            onlineDrivers.ForEach(d => d.Photo = _fileUrlBuilderService.GetMediaUrl(d.Photo, MediaType.userImage));

            return onlineDrivers;
        }

        private double CalculatePriceByRating(double basePrice, double rating)
        {
            var multiplier = 1 + (rating - 3) * 0.1;
            return Math.Round(basePrice * multiplier, 2);
        }
        public async Task<int> ChooseDriver(ChooseDriverDto chooseDriverDto, string lan)
        {

            var resident = await _residentRepository.GetByIdAsync(chooseDriverDto.PassengerId);
            if (resident == null)
                throw new NotFoundException(LocalizationKey.ResidentNotFound);

            var hasActiveRide = await _rideRepository.IsHasActiveRide(chooseDriverDto.PassengerId);
            if (hasActiveRide)
                throw new BadRequestException(LocalizationKey.ResidentHasActiveRide);

            var driver = await _driverRepository.GetByIdAsync(chooseDriverDto.DriverId);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);

            if (driver.DriverStatus == DriverStatus.OnTrip)
                throw new BadRequestException(LocalizationKey.DriverOnTrip);

            var estimateResult = EstimateRide(new CalculateRideDto
            {
                PickupLatitude = chooseDriverDto.PickupLatitude,
                PickupLongitude = chooseDriverDto.PickupLongitude,
                DropoffLatitude = chooseDriverDto.DropoffLatitude,
                DropoffLongitude = chooseDriverDto.DropoffLongitude,
                VehicleType = chooseDriverDto.VehicleType
            });

            var price = CalculatePriceByRating(estimateResult.EstimatedPrice, driver.Rating);

            var ride = new RideModel
            {
                ResidentId = chooseDriverDto.PassengerId,
                DriverId = chooseDriverDto.DriverId,
                PickupLatitude = chooseDriverDto.PickupLatitude,
                PickupLongitude = chooseDriverDto.PickupLongitude,
                DropoffLatitude = chooseDriverDto.DropoffLatitude,
                DropoffLongitude = chooseDriverDto.DropoffLongitude,
                Date = _dateTimeHelper.Now,
                Status = RideStatus.Pending,
                price = price,
                Distance = estimateResult.Distance,
                ServiceProviderType = ServiceProviderType.Driver,
                PickUpPlace = chooseDriverDto.PickUpPlace,
                DropOffPlace = chooseDriverDto.DropOffPlace
            };

            await _rideRepository.AddAsync(ride);
            await _rideRepository.SaveChangesAsync();

            var residentPhotoUrl = _fileUrlBuilderService.GetMediaUrl(resident.ProfilePhoto, MediaType.userImage);

            var metadata = new Dictionary<string, string>
    {
        { "Distance", ride.Distance.ToString("0.0") },
        { "Price", ride.price.ToString("0.0") }
    };

            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    chooseDriverDto.DriverId,
                    NotificationType.newRideRequest,
                    ride.Id.ToString(),
                    residentPhotoUrl,
                    lan,
                    metadata
                ));

            Hangfire.BackgroundJob.Schedule<DriverFunctions>(
                x => x.CheckRideAcceptance(ride.Id),
                TimeSpan.FromMinutes(2)
            );

            return ride.Id;
        }
        public async Task<int> RejectRide(int rideId, string driverId, string lan)
        {

            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);

            if (ride.DriverId != driverId)
                throw new BadRequestException(LocalizationKey.Unauthorized);

            if (ride.Status != RideStatus.Pending)
                throw new BadRequestException(LocalizationKey.RideNotAvailable);

            await _rideRepository.UpdateRideStatusAsync(rideId, RideStatus.Rejected, null);
            var target = string.Concat(ride.Id, " , ", ride.DriverId);
            var driver = await _driverRepository.GetByIdAsync(ride.DriverId);

            var metadata = new Dictionary<string, string>
    {
        { "DriverName", driver?.FullName ?? "" }
    };
            driver.ProfilePhoto = _fileUrlBuilderService.GetMediaUrl(driver.ProfilePhoto, MediaType.userImage);

            BackgroundJob.Enqueue<NotificationFunction>(
                x => x.sendNotification(
                    ride.ResidentId,
                    NotificationType.rideRejected,
                   target,
                    driver.ProfilePhoto,
                    lan,
                    metadata
                ));

            await _hub.Clients.User(ride.ResidentId).SendAsync("RideRejected", target);

            return ride.Id;
        }

        public async Task<int> StartRide(int rideId)
        {
            var ride = await _rideRepository.GetByIdAsync(rideId);
            if (ride == null)
                throw new NotFoundException(LocalizationKey.RideNotFound);
            await _userAuthorizationService.CheckOwnershipByIdAsync(ride.DriverId);

            if (ride.Status != RideStatus.Accepted)
                throw new BadRequestException(LocalizationKey.InvalidRideStatus);

            ride.Status = RideStatus.InProgress;
            _rideRepository.Update(ride);
            await _rideRepository.SaveChangesAsync();

            return ride.Id;
        }
    }
}