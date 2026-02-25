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
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToCreateUserEvent, lan));
        }

        [HttpGet("GetTopServiceProviders")]
        public async Task<IActionResult> GetTopServiceProviders(string userId, int top = 5, string lan = "en")
        {
            var result = await _userEventService.GetTopServiceProvidersAsync(userId, top);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetTopServiceProviders, lan, result));
        }

        [HttpGet("GetMostUsedServicesGlobally")]
        public async Task<IActionResult> GetMostUsedServicesGlobally(int top = 5, string lan = "en")
        {
            var result = await _userEventService.GetMostUsedServicesGloballyAsync(top);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetMostUsedServices, lan, result));
        }

        [HttpGet("GetServiceProviderResponse")]
        public async Task<IActionResult> GetServiceProviderResponse(int top = 5, string lan = "en")
        {
            var result = await _userEventService.ServiceProviderRsponse(top);
            return Ok(ResponseHelper.Success(LocalizationKey.SuccessToGetUserDashboard, lan, result));
        }
    }
}
