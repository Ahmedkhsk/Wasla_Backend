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
        [HttpPost("request")]
        public async Task<IActionResult> RequestRide(RequestRideDto requestRideDto, string lan = "en")
        {
            var result = await _rideServices.RequestRide(requestRideDto);
            return Ok(ResponseHelper.Success(LocalizationKey.RequestRideSuccessfully, lan, result));
        }
        [HttpGet("/{id}")]
        public async Task<IActionResult> GetRideById(int id, string lan = "en")
        {
            var result = await _rideServices.GetrideDetails(id);
            return Ok(ResponseHelper.Success(LocalizationKey.GetRideByIdSuccessfully, lan, result));
        }
        [HttpPut("accept/{id}")]
        public async Task<IActionResult> AcceptRide(int id, string driverId, string lan = "en")
        {
            var result = await _rideServices.AcceptRide(id, driverId);
            return Ok(ResponseHelper.Success(LocalizationKey.AcceptRideSuccessfully, lan, result));
        }
        [HttpPut("complete/{id}")]
        public async Task<IActionResult> CompleteRide(int id, string lan = "en")
        {
            var result = await _rideServices.CompleteRide(id);
            return Ok(ResponseHelper.Success(LocalizationKey.CompleteRideSuccessfully, lan, result));
        }
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelRide(int id, string lan = "en")
        {
            var result = await _rideServices.CancelRide(id);
            return Ok(ResponseHelper.Success(LocalizationKey.CancelRideSuccessfully, lan, result));
        }
        [HttpPut("start/{id}")]
        public async Task<IActionResult> StartRide(int id, string lan = "en")
        {
            var result = await _rideServices.StartRide(id);
            return Ok(ResponseHelper.Success(LocalizationKey.StartRideSuccessfully, lan, result));
        }
    }
}
