namespace Wasla_Backend.Models.ChatModel
{
    public class Chat
    {
        public int id { get; set; }

        public string senderId { get; set; }

        public string receiverId { get; set; }

        public ICollection<Message> messages { get; set; }
    }
}
