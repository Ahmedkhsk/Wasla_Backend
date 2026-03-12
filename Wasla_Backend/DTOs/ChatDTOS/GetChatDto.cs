namespace Wasla_Backend.DTOs.ChatDTOS
{
    public class GetChatDto : PaginationParams
    {
        public string senderId { get; set; }
        public string receiverId { get; set; }
    }
}
