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
        public async Task<IActionResult> CompleteRegister([FromForm] DriverCompleteRegisterDto driverCompleteRegisterDto,string lan="en")
        {
            await _driverService.CompleteRegister(driverCompleteRegisterDto);
            return Ok(ResponseHelper.Success(LocalizationKey.DriverCompleteRegisterSuccess,lan));
        }
    }
}
