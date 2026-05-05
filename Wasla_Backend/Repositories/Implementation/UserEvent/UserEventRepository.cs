using Wasla_Backend.data;

namespace Wasla_Backend.Repositories.Implementation
{
    public class UserEventRepository
        : GenericRepository<UserEvent>, IUserEventRepository
    {
        private readonly Context _context;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public UserEventRepository(Context context , IFileUrlBuilderService fileUrlBuilderService) : base(context)
        {
            _context = context;
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<List<ServiceProviderEventResponse>> GetTopServiceProvidersAsync(string userId, int top)
        {
            var query = await _context.UserEvents
                .Include(x => x.serviceProvider)
                .Where(x => x.userId == userId && x.serviceProvider.Status == UserStatus.Active)
                .GroupBy(x => x.serviceProviderId)
                .Select(g => new
                {
                    Id = g.Key,
                    Count = g.Count(),
                    Provider = g.First().serviceProvider
                })
                .OrderByDescending(x => x.Count)
                .Take(top)
                .ToListAsync();

            var result = query.Select(x => new ServiceProviderEventResponse
            {
                id = x.Id,
                name = x.Provider.BusinessName ?? x.Provider.FullName,
                description = x.Provider.Description,
                image = _fileUrlBuilderService.GetMediaUrl(x.Provider.ProfilePhoto,MediaType.userImage),
                rating = x.Provider.Rating,
                roleName = _context.UserRoles
                    .Where(ur => ur.UserId == x.Id)
                    .Join(_context.Roles,
                          ur => ur.RoleId,
                          r => r.Id,
                          (ur, r) => r.Name)
                    .FirstOrDefault()
            }).ToList();

            return result;
        }
        public async Task<List<ServiceProviderEventResponse>> GetMostUsedServicesGloballyAsync(int top)
        {
            var groupedData = await _context.UserEvents
                .Include(x => x.serviceProvider)
                .Where(x => x.serviceProvider.Status == UserStatus.Active)
                .GroupBy(e => e.serviceProviderId)
                .Select(g => new
                {
                    Id = g.Key,
                    Count = g.Count(),
                    Provider = g.First().serviceProvider
                })
                .OrderByDescending(x => x.Count)
                .Take(top)
                .AsNoTracking()
                .ToListAsync();

            var roles = await _context.UserRoles
                .Join(_context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => new { ur.UserId, r.Name })
                .AsNoTracking()
                .ToListAsync();

            return groupedData.Select(x => new ServiceProviderEventResponse
            {
                id = x.Id,
                name = x.Provider.BusinessName ?? x.Provider.FullName,
                description = x.Provider.Description,
                image = _fileUrlBuilderService.GetMediaUrl(x.Provider.ProfilePhoto, MediaType.userImage),
                rating = x.Provider.Rating,
                roleName = roles.FirstOrDefault(r => r.UserId == x.Id)?.Name
            }).ToList();
        }

        public async Task<List<ServiceProviderEventResponse>> GetTopServicesByStatusAsync(UserEventEnum status, int top)
        {
            var groupedData = await _context.UserEvents
                .Include(x => x.serviceProvider)
                .Where(x => x.eventType == status && x.serviceProvider.Status == UserStatus.Active)
                .GroupBy(x => x.serviceProviderId)
                .Select(g => new
                {
                    Id = g.Key,
                    Count = g.Count(),
                    Provider = g.First().serviceProvider
                })
                .OrderByDescending(x => x.Count)
                .Take(top)
                .AsNoTracking()
                .ToListAsync();

            var roles = await _context.UserRoles
                .Join(_context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => new { ur.UserId, r.Name })
                .AsNoTracking()
                .ToListAsync();

            return groupedData.Select(x => new ServiceProviderEventResponse
            {
                id = x.Id,
                name = x.Provider.BusinessName ?? x.Provider.FullName,
                description = x.Provider.Description,
                image = _fileUrlBuilderService.GetMediaUrl(x.Provider.ProfilePhoto, MediaType.userImage),
                rating = x.Provider.Rating,
                roleName = roles.FirstOrDefault(r => r.UserId == x.Id)?.Name
            }).ToList();
        }
        public async Task<List<ServiceProviderConversionResponse>> GetConversionRatesByRoleAsync()
        {
            var query =
                from e in _context.UserEvents
                join ur in _context.UserRoles
                    on e.serviceProviderId equals ur.UserId
                join r in _context.Roles
                    on ur.RoleId equals r.Id
                select new
                {
                    RoleName = r.Name,
                    EventType = e.eventType
                };

            return await query
                .GroupBy(x => x.RoleName)
                .Select(g => new ServiceProviderConversionResponse
                {
                    roleName = g.Key,
                    views = g.Count(x => x.EventType == UserEventEnum.view_details),
                    bookings = g.Count(x => x.EventType == UserEventEnum.booking),
                    conversionRate =
                        g.Count(x => x.EventType == UserEventEnum.view_details) == 0
                        ? 0
                        : (double)g.Count(x => x.EventType == UserEventEnum.booking)
                          / g.Count(x => x.EventType == UserEventEnum.view_details) * 100
                })
                .OrderByDescending(x => x.conversionRate)
                .ToListAsync();
        }

        public async Task<int> CountEventsForProviderAsync(string serviceProviderId)
        {
            return await _context.UserEvents
                .CountAsync(e => e.serviceProviderId == serviceProviderId);
        }

        public async Task<List<UserEvent>> GetUserEventsAsync(string userId)
        {
            return await _context.UserEvents
                .Include(x => x.serviceProvider)
                .Where(e => e.userId == userId&&e.serviceProvider.Status == UserStatus.Active)
                .OrderByDescending(e => e.timestamp)
                .ToListAsync();
        }
    }
}