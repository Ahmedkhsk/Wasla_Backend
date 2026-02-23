using Wasla_Backend.data;

namespace Wasla_Backend.Repositories.Implementation
{
    public class UserEventRepository
        : GenericRepository<UserEvent>, IUserEventRepository
    {
        private readonly Context _context;

        public UserEventRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ServiceProviderEventResponse>>  GetTopServiceProvidersAsync(string userId, int top)
        {
            return await _context.UserEvents
                .Where(x => x.userId == userId)
                .GroupBy(x => x.serviceProviderId)
                .OrderByDescending(g => g.Count())
                .Take(top)
                .Select(g => new ServiceProviderEventResponse
                {
                    id = g.Key,
                    name = g.First().serviceProvider.FullName,
                    description = g.First().serviceProvider.Description,
                    image = g.First().serviceProvider.ProfilePhoto,
                    rating = g.First().serviceProvider.Rating,
                    roleName = _context.UserRoles
                                .Where(ur => ur.UserId == g.Key)
                                .Join(_context.Roles,
                                      ur => ur.RoleId,
                                      r => r.Id,
                                      (ur, r) => r.Name)
                                .FirstOrDefault()
                })
                .ToListAsync();
        }
        public async Task<List<ServiceProviderEventResponse>> GetMostUsedServicesGloballyAsync(int top)
        {
            return await _context.UserEvents
                .GroupBy(e => e.serviceProviderId)
                .OrderByDescending(g => g.Count())
                .Take(top)
                .Select(g => new ServiceProviderEventResponse
                {
                    id = g.Key,
                    name = g.First().serviceProvider.FullName,
                    description = g.First().serviceProvider.Description,
                    image = g.First().serviceProvider.ProfilePhoto,
                    rating = g.First().serviceProvider.Rating,
                    roleName = _context.UserRoles
                                .Where(ur => ur.UserId == g.Key)
                                .Join(_context.Roles,
                                      ur => ur.RoleId,
                                      r => r.Id,
                                      (ur, r) => r.Name)
                                .FirstOrDefault()
                })
                .ToListAsync();
        }
        public async Task<List<ServiceProviderEventResponse>> GetTopServicesByStatusAsync(UserEventEnum status , int top)
        {
            return await _context.UserEvents
            .Where(x => x.eventType == status)
            .GroupBy(x => x.serviceProviderId)
            .OrderByDescending(g => g.Count())
            .Take(top)
            .Select(g => new ServiceProviderEventResponse
            {
                id = g.Key,
                name = g.First().serviceProvider.FullName,
                description = g.First().serviceProvider.Description,
                image = g.First().serviceProvider.ProfilePhoto,
                rating = g.First().serviceProvider.Rating,
                roleName = _context.UserRoles
                                .Where(ur => ur.UserId == g.Key)
                                .Join(_context.Roles,
                                      ur => ur.RoleId,
                                      r => r.Id,
                                      (ur, r) => r.Name)
                                .FirstOrDefault()
            })
            .ToListAsync();
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
                .Where(e => e.userId == userId)
                .OrderByDescending(e => e.timestamp)
                .ToListAsync();
        }
    }
}