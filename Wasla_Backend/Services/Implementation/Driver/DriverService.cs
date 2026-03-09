

namespace Wasla_Backend.Services.Implementation.Driver
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly string _imagePath;
        private readonly IMapper _mapper;
        private readonly string _FilePath;
        private readonly string _imagesPath;
        private readonly CacheManager _cacheManager;
        public DriverService(
            IDriverRepository driverRepository,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper,
            CacheManager cacheManager
        )
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathUser.TrimStart('/'));
            _FilePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.DriverFilePath.TrimStart('/'));
            _imagesPath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.DriverCarImagesPath.TrimStart('/'));
            _cacheManager = cacheManager;
        }

        public async Task ChangeStatus(string driverId, DriverStatus newStatus)
        {
            var affectedRows =await _driverRepository.ChangeStatus(driverId, newStatus);
            if (affectedRows == 0)
            {
                throw new NotFoundException(LocalizationKey.DriverNotFound);
            }
        }

        public async Task CompleteRegister(DriverCompleteRegisterDto driverCompleteRegisterDto)
        {
            var driver =await _driverRepository.GetDriverByGmailAsync(driverCompleteRegisterDto.Email);
            if (driver == null)
            {
                throw new NotFoundException(LocalizationKey.DriverNotFound);
            }

            var IsExist = await _driverRepository.IsExistByVehicleNumberAsync(driverCompleteRegisterDto.VehicleNumber);
            if (IsExist)
            {
                throw new BadRequestException(LocalizationKey.VehicleNumberAlreadyExists);
            }
            if(driverCompleteRegisterDto.CarImages == null || driverCompleteRegisterDto.CarImages.Count == 0)
            {
                throw new BadRequestException(LocalizationKey.CarImagesAreRequired);
            }
            if(driverCompleteRegisterDto.DriverFiles == null || driverCompleteRegisterDto.DriverFiles.Count == 0)
            {
                throw new BadRequestException(LocalizationKey.DriverFilesAreRequired);
            }

            _mapper.Map(driverCompleteRegisterDto, driver);
            if (driverCompleteRegisterDto.photo != null)
            {
                driver.ProfilePhoto=await FileOperation.SaveFile(driverCompleteRegisterDto.photo, _imagePath);
            }
            
           
                var carImageNames = new List<string>();
                foreach (var carImage in driverCompleteRegisterDto.CarImages)
                {
                    var carImageName = await FileOperation.SaveFile(carImage, _imagesPath);
                    carImageNames.Add(carImageName);
                }
                driver.images = carImageNames;
            
                var driverFileNames = new List<string>();
                foreach (var driverFile in driverCompleteRegisterDto.DriverFiles)
                {
                    var driverFileName = await FileOperation.SaveFile(driverFile, _FilePath);
                    driverFileNames.Add(driverFileName);
                }
                driver.DriverFiles = driverFileNames;

            
            driver.IsCompleteRegistration = true;
            _driverRepository.Update(driver);
            await _driverRepository.SaveChangesAsync();

        }

        public LocationDto GetDriverLocation(string driverId)
        {
            var key = $"TrackingDriver_{driverId}";
            var location = _cacheManager.Get<TrackingDriverDto>(key);
            if (location == null)
            {
                throw new NotFoundException(LocalizationKey.DriverLocationNotFound);
            }
            var locationDto = new LocationDto
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
            return locationDto;

        }

        public async Task<DriverProfileDTO> GetDriverProfileByIdAsync(string id)
        {
            var driver=await _driverRepository.GetByIdAsync(id);
            if (driver == null)
            {
                throw new NotFoundException(LocalizationKey.DriverNotFound);
            }
            var response = await _driverRepository.GetDriverProfileByIdAsync(id);
            response.profilePhoto=FileSetting.GetMediaUrl(response.profilePhoto, MediaType.userImage);
            if (response.carImages != null && response.carImages.Count > 0)
            {
                response.carImages = response.carImages.Select(image => FileSetting.GetMediaUrl(image, MediaType.DriverCarImage)).ToList();
            }
            if (response.driverFiles != null && response.driverFiles.Count > 0)
            {
                response.driverFiles = response.driverFiles.Select(file => FileSetting.GetMediaUrl(file, MediaType.DriverFilePath)).ToList();
            }
            return response;
        }

        public async Task<List<string>> GetTopNearestDriver(double latitude, double longitude)
        {
            var onlineDriversIds = await _driverRepository.GetAllOnlineDriversIds();

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

            var top5 = queue.UnorderedItems
                .Select(x => (DriverId: x.Element, Distance: -x.Priority)) 
                .OrderBy(d => d.Distance) 
                .Select(d => d.DriverId)
                .ToList();

            return top5;
        }

        public Task TrackingDriver(TrackingDriverDto trackingDriver)
        {
            var key = $"TrackingDriver_{trackingDriver.DriverId}";
            _cacheManager.Set(key, trackingDriver, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;

        }
    }
}
