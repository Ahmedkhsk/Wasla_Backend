namespace Wasla_Backend.Controllers.UserEvent
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEventController : ControllerBase
    {
        private readonly IUserEventService _userEventService;

        public UserEventController(IUserEventService userEventService)
        {
            _userEventService = userEventService;
        }

        [HttpPost("CreateUserEvent")]
        public async Task<IActionResult> CreateUserEvent(UserEventDto userEventDto, string lan = "en")
        {
            await _userEventService.CreateUserEventAsync(userEventDto);
            return Ok(ResponseHelper.Success("SuccessToCreateUserEvent", lan));
        }
    }
}
