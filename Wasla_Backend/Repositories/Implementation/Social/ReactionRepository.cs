namespace Wasla_Backend.Repositories.Implementation
{
    public class ReactionRepository : GenericRepository<Reaction>, IReactionRepository
    {
        public ReactionRepository(Context context) : base(context)
        {
        }

        public async Task<Reaction?> GetReaction(int targetId, ReactionTargetType type, string userId)
          => await _context.Reactions.AsNoTracking().FirstOrDefaultAsync(r => r.targetId == targetId && r.targetType == type && r.userId == userId);

        public async Task<int> GetReactionCount(int targetId, ReactionTargetType type)
            => await _context.Reactions.CountAsync(r => r.targetId == targetId && r.targetType == type);

        public async Task<GetReactionsResponse> GetReactionsResponse(int targetId, ReactionTargetType type)
        {
            var reactions = await _context.Reactions.Where(r => r.targetId == targetId && r.targetType == type)
                .Include(r => r.user)
                .ToListAsync();

            return new GetReactionsResponse
            {
                count = reactions.Count,
                reactions = reactions.Select(r => new ReactionRespnse
                {
                    userId = r.userId,
                    userName = r.user.FullName,
                    profilePhoto = FileSetting.GetMediaUrl(r.user.ProfilePhoto,MediaType.userImage),
                    createdAt = r.createdAt
                }).ToList()
            };
        }

        public async Task<Dictionary<int, int>> GetReactionCountsForPosts(List<int> postIds,ReactionTargetType targetType)
        {
            return await _context.Reactions
                .Where(r => postIds.Contains(r.targetId)
                         && r.targetType == targetType)
                .GroupBy(r => r.targetId)
                .Select(g => new
                {
                    PostId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.PostId, x => x.Count);
        }

    }
}
