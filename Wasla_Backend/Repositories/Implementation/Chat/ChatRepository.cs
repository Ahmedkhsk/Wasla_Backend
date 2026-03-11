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

        public async Task<PagedResult<GetChats>> GetChatss(GetGeneralDto<string> pagination)
        {
            var query = _dbSet
                .Where(c => c.senderId == pagination.id || c.receiverId == pagination.id)
                .Select(c => new
                {
                    chat = c,
                    lastMessage = c.messages
                        .OrderByDescending(m => m.sentAt)
                        .FirstOrDefault()
                })
                .Select(x => new GetChats
                {
                    chatId = x.chat.id,
                    receiverId = x.chat.receiverId,

                    messageText = x.lastMessage.messageText,
                    sentAt = x.lastMessage.sentAt,
                    readAt = x.lastMessage.readAt,
                    type = x.lastMessage.type,
                    audio = x.lastMessage.audio,
                    files = x.lastMessage.files,

                    name = x.chat.senderId == pagination.id
                        ? x.chat.receiver.FullName
                        : x.chat.sender.FullName,

                    profileReceiver = x.chat.senderId == pagination.id
                        ? x.chat.receiver.ProfilePhoto
                        : x.chat.sender.ProfilePhoto
                })
                .OrderByDescending(c => c.sentAt);

            return await query.ToPagedResultAsync(pagination.PageNumber, pagination.PageSize);
        }

        public async Task<Chat?> GetChatByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.messages)
                .FirstOrDefaultAsync(c => c.id == id);
        }
    }
}
