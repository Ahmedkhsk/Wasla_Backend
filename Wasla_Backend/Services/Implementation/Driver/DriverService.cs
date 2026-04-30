namespace Wasla_Backend.Services.Implementation.Driver
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;
        private readonly CacheManager _cacheManager;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public DriverService(
            IDriverRepository driverRepository,
            IMapper mapper,
            CacheManager cacheManager,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService
        )
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
            _cacheManager = cacheManager;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task ChangeStatus(string driverId, DriverStatus newStatus)
        {
            var affectedRows = await _driverRepository.ChangeStatus(driverId, newStatus);
            if (affectedRows == 0)
                throw new NotFoundException(LocalizationKey.DriverNotFound);
        }

        public async Task CompleteRegister(DriverCompleteRegisterDto driverCompleteRegisterDto)
        {
            var driver = await _driverRepository.GetDriverByGmailAsync(driverCompleteRegisterDto.Email);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);

            var IsExist = await _driverRepository.IsExistByVehicleNumberAsync(driverCompleteRegisterDto.VehicleNumber);
            if (IsExist)
                throw new BadRequestException(LocalizationKey.VehicleNumberAlreadyExists);

            if (driverCompleteRegisterDto.CarImages == null || driverCompleteRegisterDto.CarImages.Count == 0)
                throw new BadRequestException(LocalizationKey.CarImagesAreRequired);

            if (driverCompleteRegisterDto.DriverFiles == null || driverCompleteRegisterDto.DriverFiles.Count == 0)
                throw new BadRequestException(LocalizationKey.DriverFilesAreRequired);

            _mapper.Map(driverCompleteRegisterDto, driver);

            if (driverCompleteRegisterDto.photo != null)
                driver.ProfilePhoto = await _fileService.AddFileAsync(
                    driverCompleteRegisterDto.photo,
                    _fileUrlBuilderService.GetPath(MediaType.userImage)
                );

            driver.images = await _fileService.AddFilesAsync(
                driverCompleteRegisterDto.CarImages,
                _fileUrlBuilderService.GetPath(MediaType.DriverCarImage)
            );

            driver.DriverFiles = await _fileService.AddFilesAsync(
                driverCompleteRegisterDto.DriverFiles,
                _fileUrlBuilderService.GetPath(MediaType.DriverFilePath)
            );

            driver.IsCompleteRegistration = true;
            _driverRepository.Update(driver);
            await _driverRepository.SaveChangesAsync();
            var imageUrl = _fileUrlBuilderService.GetMediaUrl(driver.ProfilePhoto, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
    driver.Id,
    NotificationType.driverCompleteInfoScreen,
    driver.Id,
    imageUrl,       
    "en",        
    null
));
        }

        public async Task UpdateDriverProfile(UpdateDriverProfileDto dto)
        {
            var driver =await _driverRepository.GetByIdAsync(dto.Id);

            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);

            _mapper.Map(dto, driver);

            if (dto.Photo != null)
            {
                driver.ProfilePhoto = await _fileService.ReplaceFileAsync(
                    driver.ProfilePhoto,
                    dto.Photo,
                    _fileUrlBuilderService.GetPath(MediaType.userImage)
                );
            }


            if (dto.CarImages != null && dto.CarImages.Any())
            {
                _fileService.DeleteFiles(driver.images, _fileUrlBuilderService.GetPath(MediaType.DriverCarImage));

                driver.images = await _fileService.AddFilesAsync(
                    dto.CarImages,
                    _fileUrlBuilderService.GetPath(MediaType.DriverCarImage)
                );
            }


            if (dto.DriverFiles != null && dto.DriverFiles.Any())
            {
                _fileService.DeleteFiles(driver.DriverFiles, _fileUrlBuilderService.GetPath(MediaType.DriverFilePath));

                driver.DriverFiles = await _fileService.AddFilesAsync(
                    dto.DriverFiles,
                    _fileUrlBuilderService.GetPath(MediaType.DriverFilePath)
                );
            }

            await _driverRepository.SaveChangesAsync();
        }


        public LocationDto GetDriverLocation(string driverId)
        {
            var key = $"TrackingDriver_{driverId}";
            var location = _cacheManager.Get<TrackingDriverDto>(key);
            if (location == null)
                throw new NotFoundException(LocalizationKey.DriverLocationNotFound);

            return new LocationDto
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        public async Task<DriverProfileDTO> GetDriverProfileByIdAsync(string id)
        {
            var driver = await _driverRepository.GetByIdAsync(id);
            if (driver == null)
                throw new NotFoundException(LocalizationKey.DriverNotFound);

            var response = await _driverRepository.GetDriverProfileByIdAsync(id);

            response.profilePhoto = _fileUrlBuilderService.GetMediaUrl(response.profilePhoto, MediaType.userImage);

            if (response.carImages != null && response.carImages.Count > 0)
                response.carImages = response.carImages
                    .Select(image => _fileUrlBuilderService.GetMediaUrl(image, MediaType.DriverCarImage))
                    .ToList();

            if (response.driverFiles != null && response.driverFiles.Count > 0)
                response.driverFiles = response.driverFiles
                    .Select(file => _fileUrlBuilderService.GetMediaUrl(file, MediaType.DriverFilePath))
                    .ToList();

            return response;
        }

        public async Task<List<string>> GetTopNearestDriver(double latitude, double longitude, VehicleType vehicleType)
        {
            var onlineDriversIds = await _driverRepository.GetAllOnlineDriversIds(vehicleType);

            var queue = new PriorityQueue<string, double>();

            foreach (var driverId in onlineDriversIds)
            {
                var key = $"TrackingDriver_{driverId}";
                var location = _cacheManager.Get<TrackingDriverDto>(key);

                if (location == null)
                    continue;

                var distance = GeoHelper.CalculateDistance(
                    latitude,
                    longitude,
                    location.Latitude,
                    location.Longitude);

                queue.Enqueue(driverId, -distance);

                if (queue.Count > 5)
                    queue.Dequeue();
            }

            return queue.UnorderedItems
                .Select(x => (DriverId: x.Element, Distance: -x.Priority))
                .OrderBy(d => d.Distance)
                .Select(d => d.DriverId)
                .ToList();
        }

        public Task TrackingDriver(TrackingDriverDto trackingDriver)
        {
            var key = $"TrackingDriver_{trackingDriver.DriverId}";
            _cacheManager.Set(key, trackingDriver, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }
    }
}