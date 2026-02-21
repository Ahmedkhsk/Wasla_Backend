namespace Wasla_Backend.DTOs.UserEventDTOS
{
    public class UserEventDto
    {
        public string UserId { get; set; } 
        public int ServiceId { get; set; }
        public string EventType { get; set; } 
        public DateTime? Timestamp { get; set; }
    }
}
