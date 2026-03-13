namespace Wasla_Backend.Repositories.Implementation
{
    public class ReactionRepository : GenericRepository<Reaction>, IReactionRepository
    {
        private readonly IFileUrlBuilderService _fileUrlBuilderService;

        public ReactionRepository(Context context, IFileUrlBuilderService fileUrlBuilderService) : base(context)
        {
            _fileUrlBuilderService = fileUrlBuilderService;
        }

        public async Task<Reaction?> GetReaction(ToggleReactionDto dto)
            => await _context.Reactions.AsNoTracking().FirstOrDefaultAsync(r =>
                r.targetId == dto.targetId &&
                r.targetType == dto.targetType &&
                r.reactionType == dto.reactionType &&
                r.userId == dto.userId);

        public async Task<int> GetReactionCount(int targetId, ReactionTargetType type, ReactionType reactionType)
            => await _context.Reactions.CountAsync(r =>
                r.targetId == targetId &&
                r.targetType == type &&
                r.reactionType == reactionType);

        public async Task<GetReactionsResponse> GetReactionsResponse(int targetId, ReactionTargetType type, ReactionType reactionType)
        {
            var reactions = await _context.Reactions
                .Where(r => r.targetId == targetId && r.targetType == type && r.reactionType == reactionType)
                .Include(r => r.user)
                .Select(r => new
                {
                    r.userId,
                    userName = r.user.FullName,
                    profilePhoto = r.user.ProfilePhoto,
                    r.createdAt
                })
                .ToListAsync();

            return new GetReactionsResponse
            {
                count = reactions.Count,
                reactions = reactions.Select(r => new ReactionRespnse
                {
                    userId = r.userId,
                    userName = r.userName,
                    profilePhoto = _fileUrlBuilderService.GetMediaUrl(r.profilePhoto, MediaType.userImage),
                    createdAt = r.createdAt
                }).ToList()
            };
        }

        public async Task<Dictionary<int, int>> GetReactionCountsForPosts(
            List<int> postIds, ReactionTargetType targetType, ReactionType reactionType)
        {
            return await _context.Reactions
                .Where(r => postIds.Contains(r.targetId) &&
                            r.targetType == targetType &&
                            r.reactionType == reactionType)
                .GroupBy(r => r.targetId)
                .Select(g => new { PostId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.PostId, x => x.Count);
        }

        public async Task<int> GetReactionCountForUserPosts(
            string userId, ReactionTargetType targetType, ReactionType reactionType)
        {
            return await _context.Reactions
                .Where(r => r.targetType == targetType &&
                            r.reactionType == reactionType &&
                            r.userId == userId)
                .CountAsync();
        }

        public async Task<HashSet<int>> GetUserReactedPostIds(
            string userId, List<int> postIds, ReactionTargetType type, ReactionType reactionType)
        {
            return await _context.Reactions
                .Where(r =>
                    r.userId == userId &&
                    r.targetType == type &&
                    postIds.Contains(r.targetId) &&
                    r.reactionType == reactionType)
                .Select(r => r.targetId)
                .ToHashSetAsync();
        }
    }
}