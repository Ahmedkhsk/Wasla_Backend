using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "superadmin")]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;

        public SuperAdminController(ISuperAdminService superAdminService)
        {
            _superAdminService = superAdminService;
        }

        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin(AddAdminDto dto, string lan = "en")
        {
            await _superAdminService.AddAdminAsync(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.AdminAddedSuccessfully, lan));
        }

        [HttpGet("GetAdmins")]
        public async Task<IActionResult> GetAllAdmins(string lan = "en")
        {
            var result = await _superAdminService.GetAllAdminsAsync();
            return Ok(ResponseHelper.Success(LocalizationKey.AdminsRetrievedSuccessfully, lan, result));
        }

        [HttpDelete("RemoveAdmin/{adminId}")]
        public async Task<IActionResult> RemoveAdmin(string adminId, string lan = "en")
        {
            await _superAdminService.RemoveAdminAsync(adminId);
            return Ok(ResponseHelper.Success(LocalizationKey.AdminRemovedSuccessfully, lan));
        }

        [HttpPatch("ToggleAdminStatus/{adminId}")]
        public async Task<IActionResult> ToggleAdminStatus(string adminId, string lan = "en")
        {
            await _superAdminService.ToggleAdminStatusAsync(adminId);
            return Ok(ResponseHelper.Success(LocalizationKey.AdminStatusUpdatedSuccessfully, lan));
        }
    }
}
