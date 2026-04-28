namespace Wasla_Backend.DTOs.ChatDTOS
{
    public class GetUsersDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public int? chatId { get; set; }
        public string bio { get; set; }
    }
}
