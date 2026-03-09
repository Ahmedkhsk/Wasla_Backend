namespace Wasla_Backend.Repositories.Implementation
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository 
    {
        public ChatRepository(Context context) : base(context)
        {
        }

        public async Task<Chat?> GetChatByParticipantsAsync(string senderId, string receiverId)
        {
            return await _dbSet
                .Include(c => c.messages)
                .FirstOrDefaultAsync(c =>
                    (c.senderId == senderId && c.receiverId == receiverId) ||
                    (c.senderId == receiverId && c.receiverId == senderId));
        }
    }
}
