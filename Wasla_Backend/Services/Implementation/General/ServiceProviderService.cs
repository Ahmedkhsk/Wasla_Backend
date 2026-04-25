namespace Wasla_Backend.Services.Implementation.General
{
    public class ServiceProviderService : IServiceProviderService
    {
        private readonly IServiceProviderRepository _serviceProviderRepository;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        public ServiceProviderService(IServiceProviderRepository serviceProviderRepository,IFileUrlBuilderService fileUrlBuilderService)
        {
            _serviceProviderRepository = serviceProviderRepository;
            _fileUrlBuilderService = fileUrlBuilderService;
        }
        public async Task<List<ServiceProviderInfoDto>> GetAll()
        {
            var data= await _serviceProviderRepository.GetAll();
           data.ForEach(s=>s.Photo=_fileUrlBuilderService.GetMediaUrl(s.Photo, MediaType.userImage));
            return data;
        }

        public async Task<List<ServiceProviderInfoDto>> Search(string query)
        {
           var data= await _serviceProviderRepository.Search(query);
            data.ForEach(s => s.Photo = _fileUrlBuilderService.GetMediaUrl(s.Photo, MediaType.userImage));
            return data;
        }
    }
}
