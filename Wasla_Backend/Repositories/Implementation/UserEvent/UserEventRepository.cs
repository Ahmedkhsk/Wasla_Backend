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
                    rating = g.First().serviceProvider.Rating
                })
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