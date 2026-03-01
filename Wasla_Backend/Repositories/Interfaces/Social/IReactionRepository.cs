namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IReactionRepository : IGenericRepository<Reaction>
    {
        public Task<Reaction?> GetReaction(int targetId, ReactionTargetType type, string userId);
        public Task<Dictionary<int, int>> GetReactionCountsForPosts(List<int> postIds, ReactionTargetType targetType);
        public Task<int> GetReactionCount(int targetId, ReactionTargetType type);
    }
}
