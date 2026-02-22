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

        public async Task<List<string>> GetTopServiceProvidersAsync(string userId, int top)
        {
            return await _context.UserEvents
                .Where(e => e.userId == userId)
                .GroupBy(e => e.serviceProviderId)
                .OrderByDescending(g => g.Count())
                .Take(top)
                .Select(g => g.Key)
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