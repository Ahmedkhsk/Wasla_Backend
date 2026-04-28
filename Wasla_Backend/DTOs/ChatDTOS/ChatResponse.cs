namespace Wasla_Backend.DTOs.ChatDTOS
{
    public class ChatResponse
    {
        public int chatId { get; set; }
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public DateTime? lastSeenReciver { get; set; }

        public PagedResult<ChatMessageResponse> messages { get; set; }
}
}
