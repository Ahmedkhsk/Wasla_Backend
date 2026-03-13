namespace Wasla_Backend.Services.Implementation.GymService
{
    public class PackageService : IPackageService
    {
        public ServiceProviderType type => ServiceProviderType.Gym;

        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;
        private readonly IGymRepository _gymRepository;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public PackageService(
            IPackageRepository packageRepository,
            IMapper mapper,
            IGymRepository gymRepository,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService
        )
        {
            _packageRepository = packageRepository;
            _mapper = mapper;
            _gymRepository = gymRepository;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<ServiceHubData> AddPackage(AddPackageDto addPackageDto)
        {
            var serviceProvider = await _gymRepository.GetByIdAsync(addPackageDto.serviceProviderId);
            if (serviceProvider == null)
                throw new NotFoundException(LocalizationKey.ServiceProviderNotFound);

            var package = _mapper.Map<Package>(addPackageDto);
            package.PhotoUrl = await _fileService.AddFileAsync(
                addPackageDto.photo,
                _fileUrlBuilderService.GetPath(MediaType.gymImage)
            );
            package.Type = ServiceProviderType.Gym;

            await _packageRepository.AddAsync(package);
            await _packageRepository.SaveChangesAsync();

            return new ServiceHubData
            {
                serviceId = package.Id,
                serviceProviderId = package.ServiceProviderId,
            };
        }

        public async Task<ServiceHubData> RemovePackage(int PackageId)
        {
            var service = await _packageRepository.GetByIdAsync(PackageId);
            if (service == null)
                throw new NotFoundException(LocalizationKey.PackageNotFound);

            service.IsDeleted = true;
            _packageRepository.Update(service);
            await _packageRepository.SaveChangesAsync();

            return new ServiceHubData
            {
                serviceId = service.Id,
                serviceProviderId = service.ServiceProviderId,
            };
        }

        public async Task<ServiceHubData> UpdatePackage(UpdatePackageDto updatePackageDto)
        {
            var package = await _packageRepository.GetByIdAsync(updatePackageDto.id);
            if (package == null)
                throw new NotFoundException(LocalizationKey.PackageNotFound);

            _mapper.Map(updatePackageDto, package);

            package.PhotoUrl = await _fileService.ReplaceFileAsync(
                package.PhotoUrl,
                updatePackageDto.photo,
                _fileUrlBuilderService.GetPath(MediaType.gymImage)
            );

            _packageRepository.Update(package);
            await _packageRepository.SaveChangesAsync();

            return new ServiceHubData
            {
                serviceId = package.Id,
                serviceProviderId = package.ServiceProviderId,
            };
        }

        public async Task<List<PackageInfoDto>> GetAllPackages(string serviceProviderId)
        {
            return await _packageRepository.GetPackagesByServiceProviderId(serviceProviderId);
        }
    }
}