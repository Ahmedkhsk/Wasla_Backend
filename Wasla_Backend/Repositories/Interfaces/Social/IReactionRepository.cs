namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IReactionRepository : IGenericRepository<Reaction>
    {
        public Task<Reaction> GetReaction(int targetId, ReactionTargetType type, string userId);
    }
}
