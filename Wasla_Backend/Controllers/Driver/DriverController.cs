using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.Driver
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "driver")]
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

        [HttpPut("UpdateDriverProfile")]
        public async Task<IActionResult> UpdateDriverProfile([FromForm] UpdateDriverProfileDto dto, string lan = "en")
        {
            await _driverService.UpdateDriverProfile(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.UpdateDriverProfileSuccess, lan));
        }

        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(string driverId, DriverStatus newStatus, string lan = "en")
        {
            await _driverService.ChangeStatus(driverId, newStatus);
            return Ok(ResponseHelper.Success(LocalizationKey.ChangeDriverStatusSuccess, lan));
        }
        [HttpPost("TrackingDriver")]
        public async Task<IActionResult> TrackingDriver(TrackingDriverDto trackingDriver, string lan = "en")
        {
            await _driverService.TrackingDriver(trackingDriver);
            return Ok(ResponseHelper.Success(LocalizationKey.TrackingDriverSuccess, lan));
        }
        [HttpGet("GetDriverLocation")]
        public async Task<IActionResult> GetDriverLocation(string driverId, string lan = "en")
        {
            var location = _driverService.GetDriverLocation(driverId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetDriverLocationSuccess, lan, location));
        }
        [HttpGet("GetTopNearestDriver")]
        public async Task<IActionResult> GetTopNearestDriver(double latitude, double longitude,VehicleType vehicleType, string lan = "en")
        {
            var driversIds = await _driverService.GetTopNearestDriver(latitude, longitude,vehicleType);
            return Ok(ResponseHelper.Success(LocalizationKey.GetTopNearestDriverSuccess, lan, driversIds));
        }
    }
}