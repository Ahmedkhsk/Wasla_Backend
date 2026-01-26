namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IDoctorServiceService _doctorServiceService;

        public ServiceController(IDoctorServiceService doctorServiceService)
        {
            _doctorServiceService = doctorServiceService;
        }

        [HttpPost("AddService")]
        public async Task<IActionResult> AddService(ServiceDto addServiceDto,string lan = "en")
        {
            await _doctorServiceService.AddServiceAsync(addServiceDto);
            return Ok(ResponseHelper.Success("ServiceAddedSuccessfully", lan));
        }

        [HttpPut("UpdateService")]
        public async Task<IActionResult> UpdateService(UpdateServiceDto updateServiceDto, string lan = "en")
        {
            await _doctorServiceService.UpdateServiceAsync(updateServiceDto);
            return Ok(ResponseHelper.Success("ServiceUpdatedSuccessfully", lan));
        }

        [HttpGet("GetServices/{doctorId}")]
        public async Task<IActionResult> GetServices(string doctorId, string lan = "en")
        {
            var services = await _doctorServiceService.GetServices(doctorId, lan);
            return Ok(ResponseHelper.Success("FetchServicesSuccess", lan, services));
        }

        [HttpDelete("DeleteService/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId, string lan = "en")
        {
            await _doctorServiceService.DeleteServiceAsync(serviceId);
            return Ok(ResponseHelper.Success("ServiceDeletedSuccessfully", lan));
        }
    }
}
