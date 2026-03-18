namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IMessageRepository : IGenericRepository<ChatMessage>
    {
        public Task<List<int>> MarkAsRead(int chatId, string userId, DateTime now);
    }
}
