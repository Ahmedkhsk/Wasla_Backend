
using Newtonsoft.Json;
using Wasla_Backend.DTOs.PaginationDTOS;
using Notification = Wasla_Backend.Models.GeneralModel.Notification;

namespace Wasla_Backend.Services.Implementation.General
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly  IGenericRepository<ApplicationUser> _userRepository;
        private readonly IDateTimeHelper _dateTimeHelper ;
        private readonly IFirebaseService _firebaseService;
        private readonly IUserAuthorizationService _userAuthorizationService;
        public NotificationService(INotificationRepository notificationRepository, IGenericRepository<ApplicationUser> userRepository,
            IDateTimeHelper dateTimeHelper ,IFirebaseService firebaseService,IUserAuthorizationService userAuthorizationService)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _dateTimeHelper = dateTimeHelper;
            _firebaseService = firebaseService;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task SendAndSaveNotificationAsync(
      string userId,
      NotificationType type,
      string referenceId,
      string imageUrl,
      string language = "en",
      Dictionary<string, string>? metadata = null)
        {
            var (title, body) = NotificationTemplateEngine.Generate(type, language, metadata);

            var userTopic = $"User_{userId}";
            await _firebaseService.SendToTopicAsync(userTopic, title, body, referenceId, type);

            if(type!=NotificationType.messageReceived)
            {
                var metadataJson = metadata != null
                ? JsonConvert.SerializeObject(metadata)
                : null;

                var notification = new Notification
                {
                    UserId = userId,
                    Type = type,
                    ReferenceId = referenceId,
                    MetadataJson = metadataJson,
                    CreatedAt = _dateTimeHelper.Now,
                    ImageUrl = imageUrl
                };

                await _notificationRepository.AddAsync(notification);
                await _notificationRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteNotificationByNotificationIdAsync(int notificationId)
        {
            var notification =await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new NotFoundException(LocalizationKey.NotificationNotFound);
            await _userAuthorizationService.CheckOwnershipByIdAsync(notification.UserId);
            await _notificationRepository.DeleteNotificationByNotificationIdAsync(notificationId);
            await _notificationRepository.SaveChangesAsync();
        }



        public async Task<PagedResult<NotificationResponseDto>> GetNotificationsByUserIdAsync(string userId, int pageNumber, int pageSize,string lan)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(userId);
            var user =await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);
            var notifications =await _notificationRepository.GetNotificationsByUserIdAsync(userId,pageNumber,pageSize,lan);
            var count = await _notificationRepository.CountAllNotificationByuserId(userId);
            return new PagedResult<NotificationResponseDto>
            {
                Data = notifications.ToList(),
                TotalCount = count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task MarkAllAsSeenByUserIdAsync(string userId)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(userId);
            var user =await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);
           await _notificationRepository.MarkAllAsSeenByUserIdAsync(userId);
        }

        public async Task MarkAsSeenAsync(int notificationId)
        {
            var notification =await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new NotFoundException(LocalizationKey.NotificationNotFound);
            await _userAuthorizationService.CheckOwnershipByIdAsync(notification.UserId);
            await _notificationRepository.MarkAsSeenAsync(notificationId);
        }

        public async Task AddNotificationAsync(CreateNotificationDto createNotificationDto)
        {
            var notification = new Notification
            {
                UserId = createNotificationDto.UserId,
             
                Type = createNotificationDto.Type,
                ReferenceId = createNotificationDto.ReferenceId,
                ImageUrl = createNotificationDto.ImageUrl,
                CreatedAt = _dateTimeHelper.Now
            };
            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task<int> GetNotificationCountByUserIdAfterLastSeenAsync(string userId)
        {
            await _userAuthorizationService.CheckOwnershipByIdAsync(userId);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);
            return await _notificationRepository.CountNotificationByuserId(userId);
        }
    }
}
