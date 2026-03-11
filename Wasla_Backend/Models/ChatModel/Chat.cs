namespace Wasla_Backend.Models.ChatModel
{
    public class Chat
    {
        public int id { get; set; }

        [ForeignKey("sender")]
        public string senderId { get; set; }

        public ApplicationUser sender { get; set; }

        [ForeignKey("receiver")]
        public string receiverId { get; set; }
        public ApplicationUser receiver { get; set; }

        public ICollection<ChatMessage> messages { get; set; }
    }
}
