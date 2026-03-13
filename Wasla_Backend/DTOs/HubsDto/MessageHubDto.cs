namespace Wasla_Backend.DTOs.HubsDto
{
    public class MessageHubDto
    {
        public int id { get; set; }

        public int chatId { get; set; }
        public string senderId { get; set; }
        public string receiverId { get; set; }

        public string? messageText { get; set; }

        public string? audio { get; set; }

        public MessageType type { get; set; }
        public bool isMine { get; set; }
        public DateTime sentAt { get; set; }

        public DateTime? readAt { get; set; }

        public bool isSent { get; set; }
        public bool isEdited { get; set; }
        public List<string>? files { get; set; }
    }
}
