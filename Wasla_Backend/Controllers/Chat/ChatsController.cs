namespace Wasla_Backend.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("Message")]
        public async Task<IActionResult> SendMessage(AddMessageDto dto, [FromQuery] LanDto lanDto)
        {
            await _chatService.AddMessage(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToAddMessage,
                                             lanDto.lan));
        }

        [HttpPut("Message")]
        public async Task<IActionResult> UpdateMessage(UpdateMessage dto)
        {
            await _chatService.UpdateMessage(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToUpdateMessage,
                                             dto.lan));
        }

        [HttpDelete("Message")]
        public async Task<IActionResult> DeleteMessage(int messageId,
                                                       string senderId,
                                                       [FromQuery] LanDto lanDto)
        {
            await _chatService.DeleteMessage(messageId, senderId);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToDeleteMessage,
                                             lanDto.lan));
        }

        [HttpPut("Bio")]
        public async Task<IActionResult> UpdteBio(UpdateBioDto dto)
        {
            await _chatService.UpdateBio(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToUpdateBio,
                                             dto.lan));
        }

        [HttpPut("MarkAsRead/{chatId}")]
        public async Task<IActionResult> MarkAsRead(int chatId, [FromQuery] LanDto lanDto)
        {
            var userId = User.GetUserId();

            await _chatService.MarkAsRead(chatId, userId);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToMarkAsRead,
                                             lanDto.lan));
        }

        [HttpDelete("Chat")]
        public async Task<IActionResult> DeleteChat(int chatId,
                                                    string userId,
                                                    [FromQuery] LanDto lanDto)
        {
            await _chatService.DeleteChat(chatId, userId);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToDeleteChat,
                                             lanDto.lan));
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUserChats([FromQuery] PaginationParams pagination)
        {
            var id = User.GetUserId();

            var result = await _chatService.getUsers(id, pagination);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetUsers,
                                             pagination.lan,
                                             result));
        }

        [HttpGet]
        public async Task<IActionResult> GetChats([FromQuery] GetGeneralWithPaginationDto<string> pagination)
        {
            var result = await _chatService.GetChats(pagination);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetChats,
                                             pagination.lan,
                                             result));
        }

        [HttpGet("UserProfile")]
        public async Task<IActionResult> GetUserProfile([FromQuery] GetGeneralDto<string> dto)
        {
            var result = await _chatService.GetUserProfile(dto.id);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetUserProfile,
                                             dto.lan,
                                             result));
        }

        [HttpGet("Chat")]
        public async Task<IActionResult> GetChat([FromQuery] GetChatDto dto)
        {
            var result = await _chatService.GetChatAsync(dto);

            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetChat,
                                             dto.lan,
                                             result));
        }
    }
}