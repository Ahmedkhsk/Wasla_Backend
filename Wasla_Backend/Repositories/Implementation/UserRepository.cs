namespace Wasla_Backend.Repositories.Implementation
{
    public class UserRepository :  IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Context _context;

        public UserRepository(UserManager<ApplicationUser> userManager,Context context) 
        {
            _userManager = userManager;
            _context = context ;
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
            => await _userManager.Users.Where(u => u.IsVerified && u.Status == UserStatus.Active).ToListAsync();
        public async Task<List<ApplicationUser>> GetUsersByIdsAsync(List<string> ids)
        {
            return await _context.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();
        }


    }

}
