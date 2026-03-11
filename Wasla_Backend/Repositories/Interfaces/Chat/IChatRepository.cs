namespace Wasla_Backend.Repositories.Interfaces
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        public Task<Chat?> GetChatByParticipantsAsync(string senderId, string receiverId);
        public Task<Chat?> GetChatByIdAsync(int id);
        public Task<PagedResult<GetChats>> GetChatss(GetGeneralDto<string> pagination);
    }
}
