namespace Wasla_Backend.Services.Implementation
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IUserRepository userRepository,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService,
            DateTimeHelper dateTimeHelper,
            IHubContext<ChatHub> hubContext
        )
        {
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _fileService = fileService;
            _fileUrlBuilderService = fileUrlBuilderService;
            _dateTimeHelper = dateTimeHelper;
            _hubContext = hubContext;
        }

        public async Task AddMessage(AddMessageDto dto)
        {
            var chat = await _chatRepository.GetChatByParticipantsAsync(dto.senderId, dto.reciverId);

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
                receiverId = dto.reciverId,
                messageText = dto.messageText,
                type = dto.type,
                sentAt = _dateTimeHelper.Now
            };

            if (dto.audio != null)
                message.audio = await _fileService.AddFileAsync(
                    dto.audio,
                    _fileUrlBuilderService.GetPath(MediaType.chatFile)
                );

            if (dto.files != null && dto.files.Any())
                message.files = await _fileService.AddFilesAsync(
                    dto.files,
                    _fileUrlBuilderService.GetPath(MediaType.chatFile)
                );

            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();

            await _hubContext.Clients.User(dto.reciverId).SendAsync("ReceiveMessage", message);
        }

        public async Task DeleteMessage(int messageId, string userId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null || message.senderId != userId)
                throw new NotFoundException(LocalizationKey.MessageNotFoundOrNoPermission);

            if (!string.IsNullOrEmpty(message.audio))
                _fileService.DeleteFile(message.audio, _fileUrlBuilderService.GetPath(MediaType.chatFile));

            if (message.files != null && message.files.Any())
                _fileService.DeleteFiles(message.files, _fileUrlBuilderService.GetPath(MediaType.chatFile));

            _messageRepository.Delete(message);
            await _messageRepository.SaveChangesAsync();

            await _hubContext.Clients
                .Users(message.senderId, message.receiverId)
                .SendAsync("MessageDeleted", messageId);
        }

        public async Task DeleteChat(int chatId, string userId)
        {
            var chat = await _chatRepository.GetChatByIdAsync(chatId);

            if (chat == null || (chat.senderId != userId && chat.receiverId != userId))
                throw new NotFoundException(LocalizationKey.ChatNotFoundOrNoPermission);

            foreach (var message in chat.messages)
            {
                if (!string.IsNullOrEmpty(message.audio))
                    _fileService.DeleteFile(message.audio, _fileUrlBuilderService.GetPath(MediaType.chatFile));

                if (message.files != null && message.files.Any())
                    _fileService.DeleteFiles(message.files, _fileUrlBuilderService.GetPath(MediaType.chatFile));
            }

            _chatRepository.Delete(chat);
            await _chatRepository.SaveChangesAsync();
        }

        public async Task UpdateBio(UpdateBioDto updateBioDto)
        {
            var user = await _userRepository.GetUserByIdAsync(updateBioDto.userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);

            user.bio = updateBioDto.bio;
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task UpdateMessage(UpdateMessage updateMessage)
        {
            var message = await _messageRepository.GetByIdAsync(updateMessage.messageId);
            if (message == null || message.senderId != updateMessage.senderId)
                throw new NotFoundException(LocalizationKey.MessageNotFoundOrNoPermission);

            message.messageText = updateMessage.messageText;
            message.type = updateMessage.type;

            var existFilesNames = _fileService.ExtractFileNames(updateMessage.existFiles);
            message.files = await _fileService.ReplaceFilesAsync(
                message.files,
                existFilesNames,
                updateMessage.newFiles,
                _fileUrlBuilderService.GetPath(MediaType.chatFile)
            );
            message.isEdited = true;

            _messageRepository.Update(message);
            await _messageRepository.SaveChangesAsync();

            await _hubContext.Clients
                .Users(message.senderId, message.receiverId)
                .SendAsync("MessageUpdated", message);
        }

        public async Task<PagedResult<GetUsersDto>> getUsers(PaginationParams pagination)
        {
            return await _userRepository.GetUsers(pagination);
        }

        public async Task<PagedResult<GetChats>> GetChats(GetGeneralWithPaginationDto<string> pagination)
        {
            var result = await _chatRepository.GetChats(pagination);

            foreach (var chat in result.Data)
            {
                chat.profileReceiver = _fileUrlBuilderService.GetMediaUrl(chat.profileReceiver, MediaType.userImage);

                if (!string.IsNullOrEmpty(chat.audio))
                    chat.audio = _fileUrlBuilderService.GetMediaUrl(chat.audio, MediaType.chatFile);

                if (chat.files != null && chat.files.Any())
                    chat.files = chat.files
                        .Select(f => _fileUrlBuilderService.GetMediaUrl(f, MediaType.chatFile))
                        .ToList();
            }

            return result;
        }

        public async Task<UserProfileReponse> GetUserProfile(string userId)
        {
            return await _userRepository.GetUserProfile(userId);
        }

        public async Task<ChatResponse?> GetChatAsync(GetChatDto dto)
        {
            var chat = await _chatRepository.GetChatByUsingUserId(dto);

            if (chat == null)
                return null;

            foreach (var message in chat.messages.Data)
            {
                if (!string.IsNullOrEmpty(message.audio))
                    message.audio = _fileUrlBuilderService.GetMediaUrl(message.audio, MediaType.chatFile);

                if (message.files != null && message.files.Any())
                    message.files = message.files
                        .Select(f => _fileUrlBuilderService.GetMediaUrl(f, MediaType.chatFile))
                        .ToList();
            }

            return chat;
        }
    }
}