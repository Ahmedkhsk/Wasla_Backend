namespace Wasla_Backend.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceProviderController : ControllerBase
    {
        private readonly IServiceProviderService _serviceProviderService;
        public ServiceProviderController(IServiceProviderService serviceProviderService)
        {
            _serviceProviderService = serviceProviderService;
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAll( int pageNumber=1, int pageSize=5, string lan = "en")
        {
            var serviceProviders = await _serviceProviderService.GetAll(pageNumber,pageSize);
            return Ok(ResponseHelper.Success(LocalizationKey.ServiceProvidersRetrievedSuccessfully, lan, serviceProviders));
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string query, int pageNumber=1, int pageSize=5, string lan = "en")
        {
            var serviceProviders = await _serviceProviderService.Search(query,pageNumber,pageSize);
            return Ok(ResponseHelper.Success(LocalizationKey.ServiceProvidersRetrievedSuccessfully, lan, serviceProviders));

        }
    }
}