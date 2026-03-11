namespace Wasla_Backend.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPut("Bio")]
        public async Task<IActionResult> UpdteBio(UpdateBioDto dto)
        {
            await _chatService.UpdateBio(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToUpdateBio, dto.lan));
        }

        [HttpDelete("Chat")]
        public async Task<IActionResult> DeleteChat(int chatId,string userId, string lan = "en")
        {
            await _chatService.DeleteChat(chatId,userId);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToDeleteChat, lan));
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUserChats([FromQuery] GetUsersInChatDto pagination)
        {
            var result = await _chatService.getUsers(pagination);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetUsers, pagination.lan, result));
        }

        [HttpGet("Chats")]
        public async Task<IActionResult> GetChats([FromQuery] GetGeneralDto<string> pagination)
        {
            var result = await _chatService.GetChats(pagination);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetChats, pagination.lan, result));
        }
    }
}
