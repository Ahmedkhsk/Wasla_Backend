namespace Wasla_Backend.Models.ChatModel
{
    public class ChatMessage
    {
        public int id { get; set; }

        public int chatId { get; set; }

        [ForeignKey("chatId")]
        public Chat Chat { get; set; }

        public string senderId { get; set; }
        public string receiverId { get; set; }

        public string? messageText { get; set; }

        public string? audio { get; set; }

        public MessageType type { get; set; }

        public DateTime sentAt { get; set; }

        public DateTime? readAt { get; set; }

        public bool isSent { get; set; } 
        public bool isEdited { get; set; } = false;
        public string? fileJson { get; set; }

        [NotMapped]
        public List<string> files
        {
            get => fileJson == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(fileJson);
            set => fileJson = JsonSerializer.Serialize(value);
        }
    }

}
