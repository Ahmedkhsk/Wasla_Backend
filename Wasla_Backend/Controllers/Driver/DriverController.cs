using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.Driver
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpPost("CompleteRegister")]
        public async Task<IActionResult> CompleteRegister([FromForm] DriverCompleteRegisterDto driverCompleteRegisterDto, string lan = "en")
        {
            await _driverService.CompleteRegister(driverCompleteRegisterDto);
            return Ok(ResponseHelper.Success(LocalizationKey.DriverCompleteRegisterSuccess, lan));
        }
        [HttpGet("GetDriverProfileById")]
        public async Task<IActionResult> GetDriverProfileById(string id, string lan = "en")
        {
            var driverProfile = await _driverService.GetDriverProfileByIdAsync(id);
            return Ok(ResponseHelper.Success(LocalizationKey.GetDriverProfileSuccess, lan, driverProfile));
        }
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(string driverId, DriverStatus newStatus, string lan = "en")
        {
            await _driverService.ChangeStatus(driverId, newStatus);
            return Ok(ResponseHelper.Success(LocalizationKey.ChangeDriverStatusSuccess, lan));
        }
    }
}