using Wasla_Backend.DTOs.RoleDTOS;

namespace Wasla_Backend.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> AddRoleAsync(AddRoleDto role);
        Task<IList<string>> GetUserRolesAsync(string userId);
        public Task<IList<RolesResponse>> GetAllRolesAsync(string lan);
        public Task<IdentityResult> DeleteRole(string roleId);
    }
}
