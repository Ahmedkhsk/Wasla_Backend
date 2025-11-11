using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentController : ControllerBase
    {
        public IResidentService _residentService;
        public ResidentController(IResidentService residentService)
        {
            _residentService = residentService;
        }
        [HttpPost("CompleteRegister")]
        public async Task<IActionResult> CompleteRegister([FromForm] ResidentCompleteRegisterDto model, string lan = "en")
        {
            
                if (!ModelState.IsValid)
                    return BadRequest(ResponseHelper.Fail("InvalidData", lan, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                await _residentService.CompleteResidentRegister(model);
                return Ok(ResponseHelper.Success("CompleteResidentRegisterSuccess", lan));
            

        }
    }
}
