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
        public async Task<PagedResult<ServiceProviderInfoDto>> GetAll(int pageNumber, int pageSize)
        {
            var result= await _serviceProviderRepository.GetAll(pageNumber, pageSize);
            result.Data.ForEach(s=>s.Photo=_fileUrlBuilderService.GetMediaUrl(s.Photo, MediaType.userImage));
            return result;
        }

        public async Task<PagedResult<ServiceProviderInfoDto>> Search(string query,int pageNumber, int pageSize)
        {
           var result = await _serviceProviderRepository.Search(query, pageNumber,pageSize);
            result.Data.ForEach(s => s.Photo = _fileUrlBuilderService.GetMediaUrl(s.Photo, MediaType.userImage));
            return result;
        }
    }
}
