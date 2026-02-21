namespace Wasla_Backend.Models
{
    public class UserEvent
    {
        public int id { get; set; }

        public string userId { get; set; } 

        public int serviceId { get; set; }

        public string eventType { get; set; }

        public DateTime timestamp { get; set; }
    }
}
