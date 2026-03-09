namespace Wasla_Backend.DTOs.ChatDTOS
{
    public class AddMessageDto
    {
        public string senderId { get; set; }
        public string reciverId { get; set; }
        public string? messageText { get; set; }
        public IFormFile? audio { get; set; }
        public MessageType type { get; set; }
        public List<IFormFile>? files { get; set; }
    }
}
