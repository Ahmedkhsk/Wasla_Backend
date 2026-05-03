using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email)!;

        public async Task<int> countUsers()
            => await _userManager.Users.CountAsync();

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
            => await _userManager.UpdateAsync(user);

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
            => await _userManager.FindByIdAsync(id);

        public async Task<IEnumerable<ApplicationUser>> GetAll()
            => await _userManager.Users.ToListAsync();

        public async Task<PagedResult<GetUsersDto>> GetUsers(string userId, PaginationParams pagination)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.search))
            {
                query = query.Where(u =>
                    u.FullName.Contains(pagination.search)   
                    );
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
                    chatId = _context.Chats.Where(c => c.receiverId == user.Id && c.senderId == userId || c.senderId == user.Id && c.receiverId == userId)
                        .Select(c => c.id)
                        .FirstOrDefault()
                }).ToList()
            };
        }
        public async Task<UserProfileReponse> GetUserProfile(string userId)
        {
            var raw = await _context.Users
                .Where(u => u.Id == userId)
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

        public async Task<List<ApplicationUser>> GetUsersByIdsAsync(List<string> ids)
        {
            return await _context.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
    => await _userManager.DeleteAsync(user);

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }

        public string GetUserPhoto(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId).ProfilePhoto;
        }
    }
}
