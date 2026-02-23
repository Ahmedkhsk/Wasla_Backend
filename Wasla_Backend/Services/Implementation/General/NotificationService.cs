
using System.Drawing.Printing;

namespace Wasla_Backend.Services.Implementation.General
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly  IGenericRepository<ApplicationUser> _userRepository;
        public NotificationService(INotificationRepository notificationRepository, IGenericRepository<ApplicationUser> userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task DeleteNotificationByNotificationIdAsync(int notificationId)
        {
            var notification =await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new NotFoundException(LocalizationKey.NotificationNotFound);
            await _notificationRepository.DeleteNotificationByNotificationIdAsync(notificationId);
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task<PagedResult<NotificationResponseDto>> GetNotificationByUserIdAfterLastSeenAsync(string userId, int pageNumber, int pageSize)
        {
            var user =await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);
            var notifications = await _notificationRepository.GetNotificationByUserIdAfterLastSeenAsync(userId, pageNumber, pageSize);
       return new PagedResult<NotificationResponseDto>
       {
           Data = notifications.ToList(),
           TotalCount =await _notificationRepository.CountNotificationByuserId(userId),
           PageNumber = pageNumber,
           PageSize = pageSize
       };

        }

        public async Task<PagedResult<NotificationResponseDto>> GetNotificationsByUserIdAsync(string userId, int pageNumber, int pageSize)
        {
           var user =await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException(LocalizationKey.UserNotFound);
            var notifications =await _notificationRepository.GetNotificationsByUserIdAsync(userId,pageNumber,pageSize);
            return new PagedResult<NotificationResponseDto>
            {
                Data = notifications.ToList(),
                TotalCount = await _notificationRepository.CountNotificationByuserId(userId),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task MarkAllAsSeenByUserIdAsync(string userId)
        {
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
            await _notificationRepository.MarkAsSeenAsync(notificationId);
        }
    }
}
