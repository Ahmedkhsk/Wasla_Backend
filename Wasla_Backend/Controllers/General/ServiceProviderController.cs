using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProviderController : ControllerBase
    {
        private readonly IServiceProviderService _serviceProviderService;
        public ServiceProviderController(IServiceProviderService serviceProviderService)
        {
            _serviceProviderService = serviceProviderService;
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAll(string lan = "en")
        {
            var serviceProviders = await _serviceProviderService.GetAll();
            return Ok(ResponseHelper.Success(LocalizationKey.ServiceProvidersRetrievedSuccessfully, lan, serviceProviders));
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string query, string lan = "en")
        {
            var serviceProviders = await _serviceProviderService.Search(query);
            return Ok(ResponseHelper.Success(LocalizationKey.ServiceProvidersRetrievedSuccessfully, lan, serviceProviders));

        }
    }
}