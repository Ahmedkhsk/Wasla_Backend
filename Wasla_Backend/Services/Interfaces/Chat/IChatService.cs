namespace Wasla_Backend.Services.Interfaces
{
    public interface IChatService
    {
        public Task AddMessage(AddMessageDto dto);
        public Task DeleteMessage(int messageId, string userId);
        public Task DeleteChat(int chatId, string userId);
        public Task UpdateBio(UpdateBioDto updateBioDto);
        public Task UpdateMessage(UpdateMessage updateMessage);
        public Task<PagedResult<GetUsersDto>> getUsers(PaginationParams pagination);
        public Task<PagedResult<GetChats>> GetChats(GetGeneralDto<string> pagination);
    }
}
    