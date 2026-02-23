using Notification = Wasla_Backend.Models.GeneralModel.Notification;

namespace Wasla_Backend.Repositories.Interfaces.General
{
    public interface INotificationRepository:IGenericRepository<Notification>
    {
        public Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(string userIdو, int pageNumber, int pageSize);
        public Task MarkAsSeenAsync(int notificationId);
        public Task MarkAllAsSeenByUserIdAsync(string userId);
        public Task<IEnumerable<NotificationResponseDto>>GetNotificationByUserIdAfterLastSeenAsync(string userIdو, int pageNumber, int pageSize);
        public Task DeleteNotificationByNotificationIdAsync(int notificationId);
        public Task<int>CountNotificationByuserId(string userId);
    }
}
