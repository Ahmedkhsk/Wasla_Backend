using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.Technician
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianController : ControllerBase
    {
        private readonly ITechnicianService _technicianService;
        public TechnicianController(ITechnicianService technicianService)
        {
            _technicianService = technicianService;
        }
        [HttpPost("CompleteRegister")]
        public async Task<IActionResult> CompleteRegister(TechnicianCompleteRegisterDto technicianCompleteRegisterDto, string lan = "en")
        {
            await _technicianService.CompleteRegisterAsync(technicianCompleteRegisterDto);
            return Ok(ResponseHelper.Success(LocalizationKey.TechnicianCompleteRegisterSuccessfully, lan));
        }
        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile(string id, string lan = "en")
        {
            var profile = await _technicianService.GetProfileById(id);
            return Ok(ResponseHelper.Success(LocalizationKey.TechnicianProfileRetrievedSuccessfully, lan, profile));
        }
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(TechnicianUpdateProfileDto technicianUpdateProfileDto, string lan = "en")
        {
            await _technicianService.UpdateProfile(technicianUpdateProfileDto);
            return Ok(ResponseHelper.Success(LocalizationKey.TechnicianProfileUpdatedSuccessfully, lan));
        }
        [HttpGet("Specializations")]
        public IActionResult GetSpecialization(string lan = "en")
        {
            var specializations = _technicianService.GetSpecializations(lan);
            return Ok(ResponseHelper.Success(LocalizationKey.TechnicianSpecialtiesRetrievedSuccessfully, lan, specializations));
        }
        [HttpGet("GetBySpecialty")]
        public async Task<IActionResult> GetBySpecialty(TechnicianSpecialty? specialty, int pageNumber = 1, int pageSize = 10, string lan = "en")
        {
            var technicians = await _technicianService.GetTechniciansBySpecialty(specialty, pageNumber, pageSize, lan);
            return Ok(ResponseHelper.Success(LocalizationKey.TechniciansRetrievedSuccessfully, lan, technicians));

        }
        [HttpGet("GetChart")]
        public async Task<IActionResult> GetChart(string TechnicianId, string lan = "en")
        {
            var chartData = await _technicianService.GetChartById(TechnicianId);
            return Ok(ResponseHelper.Success(LocalizationKey.TechnicianChartRetrievedSuccessfully, lan, chartData));

        }
    }
}