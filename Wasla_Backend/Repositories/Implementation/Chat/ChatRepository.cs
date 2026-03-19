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

        public async Task<PagedResult<GetChats>> GetChats(GetGeneralWithPaginationDto<string> pagination)
        {
            var query = _dbSet
                .Where(c => c.senderId == pagination.id || c.receiverId == pagination.id)
                .Where(c =>
                    (c.senderId == pagination.id && c.deletedBySenderId == null) ||
                    (c.receiverId == pagination.id && c.deletedByReceiverId == null)
                );

            if (!string.IsNullOrWhiteSpace(pagination.search))
            {
                var search = pagination.search;
                query = query.Where(c =>
                    EF.Functions.Like(
                        c.senderId == pagination.id
                            ? c.receiver.FullName
                            : c.sender.FullName,
                        $"%{search}%"
                    )
                    ||
                    c.messages
                        .OrderByDescending(m => m.sentAt)
                        .Take(1)
                        .Any(m => EF.Functions.Like(m.messageText, $"%{search}%"))
                );
            }

            var resultQuery = query
                .Select(c => new
                {
                    chat = c,
                    lastMessage = c.messages
                        .OrderByDescending(m => m.sentAt)
                        .FirstOrDefault()
                })
                .Where(x => x.lastMessage != null)
                .Select(x => new GetChats
                {
                    chatId = x.chat.id,
                    receiverId = x.lastMessage.receiverId,
                    senderId = x.lastMessage.senderId,
                    
                    UnreadMessageCount = x.chat.messages.Count(m => m.senderId != pagination.id && !m.isRead),
                    isEdit = x.lastMessage.isEdited,
                    isMine = x.lastMessage.senderId == pagination.id,
                    messageId = x.lastMessage.id,
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

            return await resultQuery.ToPagedResultAsync(pagination.PageNumber, pagination.PageSize);
        }
        
        public async Task<Chat?> GetChatByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.messages)
                .FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<ChatResponse?> GetChatByUsingUserId(GetChatDto dto)
        {
            var chat = await _context.Chats
                .Where(c =>
                    (c.senderId == dto.senderId && c.receiverId == dto.receiverId) ||
                    (c.senderId == dto.receiverId && c.receiverId == dto.senderId))
                .FirstOrDefaultAsync();

            if (chat == null)
                return null;

            var deletedAt = chat.senderId == dto.senderId
                ? chat.senderDeletedAt
                : chat.receiverDeletedAt;

            var messagesQuery = _context.Messages
                .Where(m => m.chatId == chat.id)
                .Where(m => deletedAt == null || m.sentAt > deletedAt)
                .OrderByDescending(m => m.sentAt)
                .Select(m => new ChatMessageResponse
                {
                    messageText = m.messageText,
                    audio = m.audio,
                    type = m.type,
                    senderId = m.senderId,
                    receiverId = m.receiverId,
                    isMine = m.senderId == dto.senderId,
                    messageId = m.id,
                    sentAt = m.sentAt,
                    readAt = m.readAt,
                    isEdited = m.isEdited,
                    files = m.files
                });

            return new ChatResponse
            {
                chatId = chat.id,
                senderId = dto.senderId,
                receiverId = dto.receiverId,
                messages = await messagesQuery.ToPagedResultAsync(dto.PageNumber, dto.PageSize)
            };
        }
    }
}
