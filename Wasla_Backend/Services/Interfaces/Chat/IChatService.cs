namespace Wasla_Backend.Services.Interfaces
{
    public interface IChatService
    {
        public Task AddMessage(AddMessageDto dto);
        public Task<PagedResult<GetUsersDto>> getUsers(GetUsersInChatDto pagination);

    }
}
    