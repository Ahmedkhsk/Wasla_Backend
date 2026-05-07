using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FirebaseController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;

        public FirebaseController(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("SubscribeDevice")]
        public async Task<IActionResult> SubscribeDevice(string deviceToken, string userId, string lan = "en")
        {
            await _firebaseService.SubscribeDeviceAsync(deviceToken, userId);
            return Ok(ResponseHelper.Success(LocalizationKey.UserSubscriptionSuccess, lan));
        }

        [HttpPost("UnsubscribeDevice")]
        public async Task<IActionResult> UnsubscribeDevice(string deviceToken, string userId, string lan = "en")
        {
            await _firebaseService.UnsubscribeDeviceAsync(deviceToken, userId);
            return Ok(ResponseHelper.Success(LocalizationKey.UserUnsubscriptionSuccess, lan));
        }

        [HttpPost("SendToTopic")]
        public async Task<IActionResult> SendToTopic(string topic, string title, string body, string refrenceid
            , NotificationType type, string lan = "en")
        {
            var messageId = await _firebaseService.SendToTopicAsync(topic, title, body, refrenceid, type);
            return Ok(ResponseHelper.Success(LocalizationKey.NotificationSentToTopicSuccess, lan, messageId));
        }

        [HttpPost("SendToDevice")]
        public async Task<IActionResult> SendToDevice(string deviceToken, string title, string body,string refrenceid
            ,NotificationType type , string lan = "en")
        {
            var messageId = await _firebaseService.SendToDeviceAsync(deviceToken, title, body, refrenceid, type);
            return Ok(ResponseHelper.Success(LocalizationKey.NotificationSentToDeviceSuccess, lan, messageId));
        }
    }
}