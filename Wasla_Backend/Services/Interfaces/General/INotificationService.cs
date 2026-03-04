namespace Wasla_Backend.Services.Interfaces.General
{
    public interface INotificationService
    {

        public Task<PagedResult<NotificationResponseDto>> GetNotificationsByUserIdAsync(string userId, int pageNumber, int pageSize, string lan);
        public Task MarkAsSeenAsync(int notificationId);
        public Task MarkAllAsSeenByUserIdAsync(string userId);
        public Task<int> GetNotificationCountByUserIdAfterLastSeenAsync(string userId);
        public Task DeleteNotificationByNotificationIdAsync(int notificationId);
        public Task AddNotificationAsync(CreateNotificationDto createNotificationDto);
        public Task SendAndSaveNotificationAsync(string userId, NotificationType type, string referenceId, string imageUrl,
           string language = "en", Dictionary<string, string>? metadata = null);

    }
}
