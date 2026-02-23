namespace Wasla_Backend.DTOs.NotificationDTOS
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public  NotificationType Type { get; set; }
        public string ReferenceId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsSeen { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastSeenAt { get; set; }
    }
}
