namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IReactionRepository : IGenericRepository<Reaction>
    {
        public Task<Reaction?> GetReaction(ToggleReactionDto dto);
        public Task<Dictionary<int, int>> GetReactionCountsForPosts(List<int> postIds, ReactionTargetType targetType, ReactionType reactionType);
        public Task<HashSet<int>> GetUserReactedPostIds(string userId, List<int> postIds, ReactionTargetType type, ReactionType reactionType);
        public Task<int> GetReactionCount(int targetId, ReactionTargetType type, ReactionType reactionType);
    }
}
