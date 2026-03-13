namespace Wasla_Backend.DTOs.ChatDTOS
{
    public class GetChats
    {
        public string receiverId { get; set; }
        public string senderId { get; set; }
        public int chatId { get; set; }
        public string name { get; set; }
        public string profileReceiver { get; set; }
        public string? messageText { get; set; }
        public int? messageId { get; set; }
        public bool? isMine { get; set; }
        public string? audio { get; set; }
        public bool? isEdit { get; set; }
        public MessageType? type { get; set; }
        public List<string>? files { get; set; }
        public DateTime? sentAt { get; set; }
        public DateTime? readAt { get; set; }

    }
}
