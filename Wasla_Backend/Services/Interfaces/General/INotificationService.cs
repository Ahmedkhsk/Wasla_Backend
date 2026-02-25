namespace Wasla_Backend.Services.Interfaces.General
{
    public interface INotificationService
    {

        public Task<PagedResult<NotificationResponseDto>> GetNotificationsByUserIdAsync(string userId, int pageNumber, int pageSize);
        public Task MarkAsSeenAsync(int notificationId);
        public Task MarkAllAsSeenByUserIdAsync(string userId);
        public Task<PagedResult<NotificationResponseDto>> GetNotificationByUserIdAfterLastSeenAsync(string userId, int pageNumber, int pageSize);
        public Task DeleteNotificationByNotificationIdAsync(int notificationId);
        public Task AddNotificationAsync(CreateNotificationDto createNotificationDto);

    }
}
