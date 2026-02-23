namespace Wasla_Backend.Models
{
    public class UserEvent
    {
        public int id { get; set; }

        public string userId { get; set; }

        [ForeignKey("serviceProvider")]
        public string serviceProviderId { get; set; }

        public ServiceProvider serviceProvider { get; set; }

        public UserEventEnum eventType { get; set; }

        public DateTime timestamp { get; set; }
    }
}
