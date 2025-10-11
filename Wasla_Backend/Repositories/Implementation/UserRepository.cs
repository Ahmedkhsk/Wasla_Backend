namespace Wasla_Backend.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
            => await _userManager.CreateAsync(user, password);
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email)!;
  
    }

}
