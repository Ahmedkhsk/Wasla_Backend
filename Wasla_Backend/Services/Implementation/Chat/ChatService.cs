namespace Wasla_Backend.Services.Implementation
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly DateTimeHelper _dateTimeHelper;

        public ChatService(IChatRepository chatRepository, IMessageRepository messageRepository, IUserRepository userRepository,
                            IFileService fileService, DateTimeHelper dateTimeHelper)
        {
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _fileService = fileService;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task AddMessage(AddMessageDto dto)
        {
            var chat = await _chatRepository.GetChatByParticipantsAsync(dto.senderId,dto.reciverId);
            
            if (chat == null)
            {
                chat = new Chat
                {
                    senderId = dto.senderId,
                    receiverId = dto.reciverId,
                };
                await _chatRepository.AddAsync(chat);
                await _chatRepository.SaveChangesAsync();
            }
            
            var message = new ChatMessage
            {
                chatId = chat.id,
                senderId = dto.senderId,
                messageText = dto.messageText,
                type = dto.type,
                sentAt = _dateTimeHelper.Now
            };
            
            if (dto.audio != null)
            {
                message.audio = await _fileService.AddFileAsync(dto.audio, FileSetting.FilesChat);
            }
            if (dto.files != null && dto.files.Any())
            {
                message.files = await _fileService.AddFilesAsync(dto.files, FileSetting.FilesChat);
            }
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetUsersDto>> getUsers()
        {
            return await _userRepository.GetUsers();
        }
    }
}
