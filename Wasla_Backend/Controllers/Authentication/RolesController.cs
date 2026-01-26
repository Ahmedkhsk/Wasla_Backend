using Wasla_Backend.DTOs.RoleDTOS;

namespace Wasla_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRole(AddRoleDto addRoleDto, string lan = "en")
        {
            if (string.IsNullOrWhiteSpace(addRoleDto.Value))
                return BadRequest(ResponseHelper.Fail("RoleNameRequired", lan));

            var result = await _roleService.AddRoleAsync(addRoleDto);

            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail("RoleAddFailed", lan, result.Errors));

            return Ok(ResponseHelper.Success("RoleAddedSuccessfully", lan, addRoleDto));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId, string lan = "en")
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest(ResponseHelper.Fail("UserIdRequired", lan));

            var roles = await _roleService.GetUserRolesAsync(userId);

            if (roles == null || !roles.Any())
                return BadRequest(ResponseHelper.Fail("NoRolesFoundForUser", lan));

            return Ok(ResponseHelper.Success("UserRolesRetrieved", lan, roles));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles(string lan = "en")
        {
            var roles = await _roleService.GetAllRolesAsync(lan);

            if (roles == null || !roles.Any())
                return BadRequest(ResponseHelper.Fail("NoRolesFound", lan));

            return Ok(ResponseHelper.Success("AllRolesRetrieved", lan, roles));
        }
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId, string lan = "en")
        {
            if (string.IsNullOrEmpty(roleId))
                return BadRequest(ResponseHelper.Fail("RoleIdRequired", lan));
            var result = await _roleService.DeleteRole(roleId);
            if (!result.Succeeded)
                return BadRequest(ResponseHelper.Fail("RoleDeletionFailed", lan, result.Errors));
            return Ok(ResponseHelper.Success("RoleDeletedSuccessfully", lan));
        }
    }
}

