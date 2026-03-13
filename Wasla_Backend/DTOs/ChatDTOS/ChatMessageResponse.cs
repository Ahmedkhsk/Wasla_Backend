namespace Wasla_Backend.DTOs.ChatDTOS
{
    public class ChatMessageResponse
    {
        public string? messageText { get; set; }

        public string? audio { get; set; }
        public int? messageId { get; set; }
        public bool? isMine { get; set; }
        public MessageType? type { get; set; }

        public DateTime? sentAt { get; set; }

        public DateTime? readAt { get; set; }
        public bool? isEdited { get; set; }
        public List<string>? files { get; set; }
    }
}
