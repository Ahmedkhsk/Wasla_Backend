namespace Wasla_Backend.Services.Implementation.GymService
{
    public class PackageService : IPackageService
    {
        public ServiceProviderType type => ServiceProviderType.Gym;
        private readonly IPackageRepository _packageRepository;
        private readonly string _imagePath;
        private readonly IMapper _mapper;
        private readonly IGymRepository _gymRepository;
        public PackageService(IWebHostEnvironment webHostEnvironment, IPackageRepository packageRepository, IMapper mapper,IGymRepository gymRepository)
        {
            _packageRepository = packageRepository;
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, FileSetting.ImagesPathGym.TrimStart('/'));
            _mapper = mapper;
            _gymRepository=gymRepository;
        }       


        public async Task<ServiceHubData> AddPackage(AddPackageDto addPackageDto)
        {
            var ServiceProvider=await _gymRepository.GetByIdAsync(addPackageDto.serviceProviderId);
            if (ServiceProvider == null)
                throw new NotFoundException("ServiceProviderNotFound");
            string photoPath = await FileOperation.SaveFile(addPackageDto.photo, _imagePath);
            var package = _mapper.Map<Package>(addPackageDto);
            package.PhotoUrl = photoPath;
            package.Type = ServiceProviderType.Gym;
            await _packageRepository.AddAsync(package);
            await _packageRepository.SaveChangesAsync();
            var serviceHubData=new ServiceHubData
            {
                serviceId = package.Id,
                serviceProviderId = package.ServiceProviderId,
            };
            return serviceHubData;
        }

        public async Task<ServiceHubData> RemovePackage(int PackageId)
        {
            var service =await _packageRepository.GetByIdAsync(PackageId);
            if (service == null)
                throw new NotFoundException("PackageNotFound");
            service.IsDeleted = true;
            _packageRepository.Update(service);
            await _packageRepository.SaveChangesAsync();
            var serviceHubData = new ServiceHubData
            {
                serviceId = service.Id,
                serviceProviderId = service.ServiceProviderId,
            };
            return serviceHubData;
        }

        public async Task<ServiceHubData> UpdatePackage(UpdatePackageDto updatePackageDto)
        {
            var package = await _packageRepository.GetByIdAsync(updatePackageDto.id);
            if (package == null)
                throw new NotFoundException("PackageNotFound");

            _mapper.Map(updatePackageDto, package);

            if (updatePackageDto.photo != null)
            {
                string newPhotoPath = await FileOperation.SaveFile(updatePackageDto.photo, _imagePath);

                if (!string.IsNullOrEmpty(package.PhotoUrl))
                {
                    string oldPhotoFullPath = Path.Combine(_imagePath, Path.GetFileName(package.PhotoUrl));
                    if (File.Exists(oldPhotoFullPath))
                    {
                        File.Delete(oldPhotoFullPath);
                    }
                }

                package.PhotoUrl = newPhotoPath;
            }

            _packageRepository.Update(package);
            await _packageRepository.SaveChangesAsync();
            var serviceHubData = new ServiceHubData
            {
                serviceId = package.Id,
                serviceProviderId = package.ServiceProviderId,
            };
            return serviceHubData;
        }

        public async Task<List<PackageInfoDto>> GetAllPackages(string serviceProviderId)
        {
           return await _packageRepository.GetPackagesByServiceProviderId(serviceProviderId);
        }
    }
}
