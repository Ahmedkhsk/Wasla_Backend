namespace Wasla_Backend.Repositories.Implementation
{
    public class MessageRepository : GenericRepository<ChatMessage>, IMessageRepository
    {
        public MessageRepository(Context context) : base(context)
        {
        }
    }
}
