namespace Wasla_Backend.DTOs.UserEventDTOS
{
    public class UserEventDto
    {
        public string userId { get; set; } 
        public string serviceProviderId { get; set; }
        public UserEventEnum eventType { get; set; } 
    }
}
