namespace Wasla_Backend.DTOs.NotificationDTOS
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; }
        public NotificationType Type { get; set; }
        public string ReferenceId { get; set; }
        public string ImageUrl { get; set; }
    }
}
