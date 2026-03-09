namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IUserRepository 
    {
        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        public Task<ApplicationUser> GetUserByEmailAsync(string email);
        public Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        public Task<ApplicationUser> GetUserByIdAsync(string id);
        public Task<IEnumerable<ApplicationUser>> GetAll();
        public Task<int> countUsers();
        public Task<IEnumerable<GetUsersDto>> GetUsers();
        public Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName);
        public Task<List<ApplicationUser>> GetUsersByIdsAsync(List<string> ids);


    }
}
