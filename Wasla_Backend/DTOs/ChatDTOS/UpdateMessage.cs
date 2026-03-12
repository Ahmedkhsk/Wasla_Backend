namespace Wasla_Backend.DTOs.ChatDTOS
{
    public class UpdateMessage : LanDto
    {
        public string senderId { get; set; }
        public int messageId { get; set; }
        public string? messageText { get; set; }
        public MessageType type { get; set; }
        public List<IFormFile>? newFiles { get; set; }
        public List<string>? existFiles { get; set; }

    }
}
