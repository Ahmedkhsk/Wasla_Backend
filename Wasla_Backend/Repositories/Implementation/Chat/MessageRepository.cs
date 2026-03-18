namespace Wasla_Backend.Repositories.Implementation
{
    public class MessageRepository : GenericRepository<ChatMessage>, IMessageRepository
    {
        public MessageRepository(Context context) : base(context)
        {
        }

        public async Task<List<int>> MarkAsRead(int chatId, string userId, DateTime now)
        {
            var messageIds = await _context.Messages
                .Where(m => m.chatId == chatId &&
                            m.receiverId == userId &&
                            !m.isRead)
                .Select(m => m.id)
                .ToListAsync();

            if (!messageIds.Any())
                return messageIds;

            await _context.Messages
                .Where(m => m.chatId == chatId &&
                            m.receiverId == userId &&
                            !m.isRead)
                .ExecuteUpdateAsync(m => m
                    .SetProperty(x => x.isRead, true)
                    .SetProperty(x => x.readAt, now));

            return messageIds;
        }
    }
}
