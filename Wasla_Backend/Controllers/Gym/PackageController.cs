namespace Wasla_Backend.Controllers.Gym
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IHubContext<ServiceHub> _hub;

        public PackageController(IPackageService packageService, IHubContext<ServiceHub> hub)
        {
            _packageService = packageService;
            _hub = hub;
        }

        [Authorize(Roles = "gym")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AddPackageDto addPackageDto,
                                                [FromQuery] LanDto lanDto)
        {
            var data = await _packageService.AddPackage(addPackageDto);

            await _hub.Clients.All.SendAsync("PackageAdded", data);

            return Ok(ResponseHelper.Success(LocalizationKey.PackageAddedSuccessfully,
                                             lanDto.lan));
        }

        [Authorize(Roles = "gym")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePackageDto updatePackageDto,
                                                [FromQuery] LanDto lanDto)
        {
            var data = await _packageService.UpdatePackage(updatePackageDto);

            await _hub.Clients.All.SendAsync("PackageUpdated", data);

            return Ok(ResponseHelper.Success(LocalizationKey.PackageUpdatedSuccessfully,
                                             lanDto.lan));
        }

        [Authorize(Roles = "gym")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int ServiceID,
                                                [FromQuery] LanDto lanDto)
        {
            var data = await _packageService.RemovePackage(ServiceID);

            await _hub.Clients.All.SendAsync("PackageDeleted", data);

            return Ok(ResponseHelper.Success(LocalizationKey.PackageDeletedSuccessfully,
                                             lanDto.lan));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string serviceProviderId,
                                                [FromQuery] LanDto lanDto)
        {
            var data = await _packageService.GetAllPackages(serviceProviderId);

            return Ok(ResponseHelper.Success(LocalizationKey.PackagesRetrievedSuccessfully,
                                             lanDto.lan,
                                             data));
        }
    }
}