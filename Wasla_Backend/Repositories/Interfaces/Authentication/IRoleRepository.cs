namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IRoleRepository
    {

        public Task<bool> RoleExistsAsync(string roleName);
        public Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
        public Task<List<string>> GetRolesNameAsync();
        public Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName);
        public Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        public Task<IList<ApplicationRole>> GetAllRolesAsync();
        public Task<ApplicationRole> GetRoleByIdAsync(string roleId);
        public Task<IdentityResult> DeleteRoleAsync(ApplicationRole role);
    }
}
