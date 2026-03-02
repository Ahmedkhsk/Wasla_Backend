using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wasla_Backend.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("User/{userId}/AfterLastSeen")]
        public async Task<IActionResult> GetNotificationsAfterLastSeen(string userId, string lan = "en")
        {
            var result = await _notificationService.GetNotificationCountByUserIdAfterLastSeenAsync(userId);

            return Ok(ResponseHelper.Success(LocalizationKey.NotificationsFetched, lan, result));
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetNotifications(string userId, int pageNumber = 1, int pageSize = 20, string lan = "en")
        {
            var pagedResult = await _notificationService
                .GetNotificationsByUserIdAsync(userId, pageNumber, pageSize);

            return Ok(ResponseHelper.Success(LocalizationKey.NotificationsFetched, lan, pagedResult));
        }

        [HttpPost("{notificationId}/MarkAsSeen")]
        public async Task<IActionResult> MarkAsSeen(int notificationId, string lan = "en")
        {
            await _notificationService.MarkAsSeenAsync(notificationId);
            return Ok(ResponseHelper.Success(LocalizationKey.NotificationMarkedAsSeen, lan));
        }

        [HttpPost("User/{userId}/MarkAllAsSeen")]
        public async Task<IActionResult> MarkAllAsSeen(string userId, string lan = "en")
        {
            await _notificationService.MarkAllAsSeenByUserIdAsync(userId);
            return Ok(ResponseHelper.Success(LocalizationKey.AllNotificationsMarkedAsSeen, lan));
        }
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId, string lan = "en")
        {
            await _notificationService.DeleteNotificationByNotificationIdAsync(notificationId);
            return Ok(ResponseHelper.Success(LocalizationKey.NotificationDeleted, lan));
        }
        [HttpPost]
        public async Task<IActionResult> AddNotification(CreateNotificationDto createNotificationDto, string lan = "en")
        {
            await _notificationService.AddNotificationAsync(createNotificationDto);
            return Ok(ResponseHelper.Success(LocalizationKey.NotificationAdded, lan));
        }
    }
}
