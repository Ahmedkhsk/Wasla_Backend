namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        public Task<Chat?> GetChatByParticipantsAsync(string senderId, string receiverId);
    }
}
