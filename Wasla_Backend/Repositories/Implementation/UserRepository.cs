namespace Wasla_Backend.Repositories.Implementation
{
    public class UserRepository :  IUserRepository
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
        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
            => await _userManager.UpdateAsync(user);
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
            => await _userManager.FindByIdAsync(id);
        public async Task<IEnumerable<ApplicationUser>>GetAll()
            => await _userManager.Users.ToListAsync();



    }

}
