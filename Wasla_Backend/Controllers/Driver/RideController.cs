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
        public async Task<IActionResult> EstimateRide(CalculateRideDto calculateRideDto,string lan="en")
        {
            var result =  _rideServices.EstimateRideAsync(calculateRideDto);
            return Ok(ResponseHelper.Success(LocalizationKey.EstimateRideSuccessfully,lan,result));
        }
    }
}
