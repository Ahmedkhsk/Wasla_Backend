namespace Wasla_Backend.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Context _context;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public UserRepository(
            UserManager<ApplicationUser> userManager,
            Context context,
            IFileUrlBuilderService fileUrlBuilderService
        )
        {
            _userManager = userManager;
            _context = context;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        // ===================== Write (UserManager) =====================

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
            => await _userManager.UpdateAsync(user);

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
            => await _userManager.DeleteAsync(user);

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
            => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<IEnumerable<ApplicationUser>> GetAll()
            => await _context.Users.ToListAsync();

        public async Task<int> countUsers()
            => await _context.Users.CountAsync();

        public async Task<List<ApplicationUser>> GetUsersByIdsAsync(List<string> ids)
            => await _context.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName);

            if (role == null)
                return Enumerable.Empty<ApplicationUser>();

            return await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Join(_context.Users,
                      ur => ur.UserId,
                      u => u.Id,
                      (ur, u) => u)
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public string GetUserPhoto(string userId)
            => _context.Users.FirstOrDefault(u => u.Id == userId)?.ProfilePhoto;

        public async Task<PagedResult<GetUsersDto>> GetUsers(string userId, PaginationParams pagination)
        {
            var query = _context.Users.
                Where(u => u.Status == UserStatus.Active).AsQueryable();


            if (!string.IsNullOrWhiteSpace(pagination.search))
            {
                query = query.Where(u => u.FullName.Contains(pagination.search));
            }

            var rawQuery = query.Select(user => new
            {
                user.Id,
                user.FullName,
                user.ProfilePhoto,
                user.bio
            });

            var paged = await rawQuery.ToPagedResultAsync(
                pagination.PageNumber,
                pagination.PageSize
            );

            return new PagedResult<GetUsersDto>
            {
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount,
                Data = paged.Data.Select(user => new GetUsersDto
                {
                    id = user.Id,
                    name = user.FullName,
                    image = _fileUrlBuilderService.GetMediaUrl(user.ProfilePhoto, MediaType.userImage),
                    bio = user.bio,
                    chatId = _context.Chats
                        .Where(c =>
                            c.receiverId == user.Id && c.senderId == userId ||
                            c.senderId == user.Id && c.receiverId == userId)
                        .Select(c => c.id)
                        .FirstOrDefault()
                }).ToList()
            };
        }

        public async Task<UserProfileReponse> GetUserProfile(string userId)
        {
            var raw = await _context.Users
                .Where(u => u.Id == userId && u.Status == UserStatus.Active)
                .Select(u => new
                {
                    u.Id,
                    u.isOnline,
                    u.lastSeen,
                    u.FullName,
                    u.ProfilePhoto,
                    u.bio,
                    u.Phone
                })
                .FirstOrDefaultAsync();

            if (raw == null) return null;

            return new UserProfileReponse
            {
                id = raw.Id,
                name = raw.FullName,
                profileImage = _fileUrlBuilderService.GetMediaUrl(raw.ProfilePhoto, MediaType.userImage),
                bio = raw.bio,
                isOnline = raw.isOnline,
                lastSeen = raw.lastSeen,
                phone = raw.Phone
            };
        }
    }
}