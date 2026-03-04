

namespace Wasla_Backend.Services.Implementation.Driver
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly string _imagePath;
        private readonly IMapper _mapper;
        private readonly string _FilePath;
        private readonly string _imagesPath;
        public DriverService(
            IDriverRepository driverRepository,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper
        )
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathUser.TrimStart('/'));
            _FilePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.DriverFilePath.TrimStart('/'));
            _imagesPath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.DriverCarImagesPath.TrimStart('/'));
        }



        public async Task CompleteRegister(DriverCompleteRegisterDto driverCompleteRegisterDto)
        {
            var driver =await _driverRepository.GetDriverByGmailAsync(driverCompleteRegisterDto.Email);
            if (driver == null)
            {
                throw new NotFoundException(LocalizationKey.DriverNotFound);
            }

            var IsExist = await _driverRepository.IsExistByVehicleNumberOrLicenseNumberAsync
                (driverCompleteRegisterDto.VehicleNumber, driverCompleteRegisterDto.LicenseNumber);
            if (IsExist)
            {
                throw new BadRequestException(LocalizationKey.VehicleNumberOrLicenseNumberAlreadyExists);
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
    }
}
