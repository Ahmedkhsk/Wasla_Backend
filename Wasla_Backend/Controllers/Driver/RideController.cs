using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.Driver
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IRideServices _rideServices;
        public RideController(IRideServices rideServices)
        {
            _rideServices = rideServices;
        }
        [HttpPost("estimate")]
        public async Task<IActionResult> EstimateRide(CalculateRideDto calculateRideDto, string lan = "en")
        {
            var result = _rideServices.EstimateRide(calculateRideDto);
            return Ok(ResponseHelper.Success(LocalizationKey.EstimateRideSuccessfully, lan, result));
        }
      
        [HttpGet("GetrideDetailsForDriver/{id}")]
        public async Task<IActionResult> GetrideDetailsForDriver(int id, string lan = "en")
        {
            var result = await _rideServices.GetrideDetailsForDriver(id);
            return Ok(ResponseHelper.Success(LocalizationKey.GetRideByIdSuccessfully, lan, result));
        }

        [HttpGet("GetrideDetailsForResident/{id}")]
        public async Task<IActionResult> GetrideDetailsForRider(int id, string lan = "en")
        {
            var result = await _rideServices.GetrideDetailsForResident(id);
            return Ok(ResponseHelper.Success(LocalizationKey.GetRideByIdSuccessfully, lan, result));
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestRide(RequestRideDto dto)
        {
            var drivers = await _rideServices.RequestRide(dto);
            return Ok(drivers);
        }

        [HttpPost("choose-driver")]
        public async Task<IActionResult> ChooseDriver(ChooseDriverDto dto,  string lan = "en")
        {
            var rideId = await _rideServices.ChooseDriver(dto, lan);
            return Ok(rideId);
        }

        [HttpPost("accept/{rideId}")]
        public async Task<IActionResult> AcceptRide(int rideId,  string driverId, [FromQuery] string lan = "en")
        {
            var result = await _rideServices.AcceptRide(rideId, driverId, lan);
            return Ok(result);
        }

        [HttpPost("reject/{rideId}")]
        public async Task<IActionResult> RejectRide(int rideId,  string driverId, [FromQuery] string lan = "en")
        {
            var result = await _rideServices.RejectRide(rideId, driverId, lan);
            return Ok(result);
        }
        [HttpPut("complete/{id}")]
        public async Task<IActionResult> CompleteRide(int id, string lan = "en")
        {
            var result = await _rideServices.CompleteRide(id, lan);
            return Ok(ResponseHelper.Success(LocalizationKey.CompleteRideSuccessfully, lan, result));
        }
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelRide(int id, bool IsResident, string lan = "en")
        {
            var result = await _rideServices.CancelRide(id, IsResident, lan);
            return Ok(ResponseHelper.Success(LocalizationKey.CancelRideSuccessfully, lan, result));
        }
        [HttpPut("start/{id}")]
        public async Task<IActionResult> StartRide(int id, string lan = "en")
        {
            var result = await _rideServices.StartRide(id);
            return Ok(ResponseHelper.Success(LocalizationKey.StartRideSuccessfully, lan, result));
        }
        [HttpGet("GetUserRides/{residentId}")]
        public async Task<IActionResult> GetUserRides(string residentId, string lan = "en")
        {
            var result = await _rideServices.GetUserRides(residentId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetUserRidesSuccessfully, lan, result));
        }

        [HttpGet("GetDriverRides/{driverId}")]
        public async Task<IActionResult> GetDriverRides(string driverId, string lan = "en")
        {
            var result = await _rideServices.GetDriverRides(driverId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetDriverRidesSuccessfully, lan, result));
        }
        [HttpGet("GetDriverChart/{driverId}")]
        public async Task<IActionResult> GetDriverChart(string driverId, string lan = "en")
        {
            var result = await _rideServices.GetDriverChart(driverId);
            return Ok(ResponseHelper.Success(LocalizationKey.GetDriverChartSuccessfully, lan, result));
        }
    }
}