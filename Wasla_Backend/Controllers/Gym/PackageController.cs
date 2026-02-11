using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Wasla_Backend.Controllers.Gym
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IHubContext<ServiceHub> _hub;

        public PackageController(IPackageService packageService, IHubContext<ServiceHub> hub)
        {
            _packageService = packageService;
            _hub = hub;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AddPackageDto addPackageDto, string lan = "en")
        {
            var data = await _packageService.AddPackage(addPackageDto);
            await _hub.Clients.All.SendAsync("PackageAdded", data);
            return Ok(ResponseHelper.Success("PackageAddedSuccessfully", lan));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePackageDto updatePackageDto, string lan = "en")
        {
            var data = await _packageService.UpdatePackage(updatePackageDto);
            await _hub.Clients.All.SendAsync("PackageUpdated", data);
            return Ok(ResponseHelper.Success("PackageUpdatedSuccessfully", lan));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int ServiceID, string lan = "en")
        {
            var data = await _packageService.RemovePackage(ServiceID);
            await _hub.Clients.All.SendAsync("PackageDeleted", data);
            return Ok(ResponseHelper.Success("PackageDeletedSuccessfully", lan));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string serviceProviderId, string lan = "en")
        {
            var data = await _packageService.GetAllPackages(serviceProviderId);
            return Ok(ResponseHelper.Success("PackagesRetrievedSuccessfully", lan, data));

        }
    }
}
