namespace Wasla_Backend.Services.Implementation
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IUserRepository userRepository,
            IFileService fileService,
            IFileUrlBuilderService fileUrlBuilderService,
            IDateTimeHelper dateTimeHelper,
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

            var receiver = await _userRepository.GetUserByIdAsync(dto.reciverId);
            var sender = await _userRepository.GetUserByIdAsync(dto.senderId);
            var messageDto = new MessageHubDto
            {
                id = message.id,
                chatId = message.chatId,
                senderId = message.senderId,
                profileReceiver = _fileUrlBuilderService.GetMediaUrl(receiver.ProfilePhoto, MediaType.userImage),
                nameReceiver = receiver.FullName,
                nameSender = sender.FullName,
                profileSender = _fileUrlBuilderService.GetMediaUrl(sender.ProfilePhoto, MediaType.userImage),
                receiverId = message.receiverId,
                messageText = message.messageText,
                audio = _fileUrlBuilderService.GetMediaUrl(message.audio, MediaType.chatFile),
                type = message.type,
                isMine = true,
                sentAt = message.sentAt,
                readAt = message.readAt,
                isSent = message.isSent,
                isEdited = message.isEdited,
                files = message.files.Select(f => _fileUrlBuilderService.GetMediaUrl(f, MediaType.chatFile)).ToList()
            };

            await _hubContext.Clients
                .Users(new List<string> { message.senderId, message.receiverId })
                .SendAsync("ReceiveMessage", messageDto);
            var metadata = new Dictionary<string, string>
{
    { "SenderName", sender.FullName ?? "User" }
};
            var refernce=string.Concat(receiver.FullName, " , ",_fileUrlBuilderService.GetMediaUrl(receiver.ProfilePhoto,MediaType.userImage));
            var photo = _fileUrlBuilderService.GetMediaUrl(sender.ProfilePhoto, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
    receiver.Id,
    NotificationType.messageReceived,
    refernce,
    photo,   
    "en",
    metadata
));
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

            if (chat.senderId == userId)
            {
                chat.deletedBySenderId = userId;
                chat.senderDeletedAt = _dateTimeHelper.Now;
            }
            else
            {
                chat.deletedByReceiverId = userId;
                chat.receiverDeletedAt = _dateTimeHelper.Now;
            }

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

            var messageDto = new MessageHubDto
            {
                id = message.id,
                chatId = message.chatId,
                senderId = message.senderId,
                receiverId = message.receiverId,
                messageText = message.messageText,
                audio = _fileUrlBuilderService.GetMediaUrl(message.audio, MediaType.chatFile),
                type = message.type,
                sentAt = message.sentAt,
                readAt = message.readAt,
                isSent = message.isSent,
                isEdited = message.isEdited,
                files = message.files.Select(f => _fileUrlBuilderService.GetMediaUrl(f, MediaType.chatFile))
                        .ToList()
            };

            await _hubContext.Clients
                .Users(new List<string> { message.senderId, message.receiverId })
                .SendAsync("MessageUpdated", messageDto);
        }

        public async Task MarkAsRead(int chatId, string userId)
        {
            var chat = await _chatRepository.GetChatByIdAsync(chatId);

            if (chat == null || (chat.senderId != userId && chat.receiverId != userId))
                throw new NotFoundException(LocalizationKey.ChatNotFoundOrNoPermission);

            var otherUserId = chat.senderId == userId
                ? chat.receiverId
                : chat.senderId;

            var timeNow = _dateTimeHelper.Now;

            var messageIds = await _messageRepository.MarkAsRead(chatId, userId, timeNow);

            if (!messageIds.Any())
                return;

            await _hubContext.Clients
                .Users(new List<string> { userId, otherUserId })
                .SendAsync("MessagesRead", new
                {
                    chatId,
                    readerId = userId,
                    messageIds
                });

            await _hubContext.Clients
                .Users(new List<string> { userId, otherUserId })
                .SendAsync("ChatUpdated", new
                {
                    chatId
                });
        }

        public async Task<PagedResult<GetUsersDto>> getUsers(string id, PaginationParams pagination)
        {
            return await _userRepository.GetUsers(id, pagination);
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