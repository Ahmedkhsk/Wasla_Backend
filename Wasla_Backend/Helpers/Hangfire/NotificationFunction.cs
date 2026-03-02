namespace Wasla_Backend.Helpers.Hangfire
{
    public class NotificationFunction
    {
        private readonly INotificationService _notificationService; 
        public NotificationFunction(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task sendNotification(string userId, NotificationType type, string referenceId, string imageUrl,
            string language = "en", Dictionary<string, string>? metadata = null)
        {
            await _notificationService.SendAndSaveNotificationAsync(userId, type, referenceId, imageUrl, language, metadata);
        }
    }
}
