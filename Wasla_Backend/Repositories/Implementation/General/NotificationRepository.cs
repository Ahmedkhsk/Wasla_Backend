using Newtonsoft.Json;
using Notification = Wasla_Backend.Models.GeneralModel.Notification;

namespace Wasla_Backend.Repositories.Implementation.General
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly DateTimeHelper _dateTimeHelper;
        public NotificationRepository(Context context, DateTimeHelper dateTimeHelper) : base(context)
        {
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task DeleteNotificationByNotificationIdAsync(int notificationId)
        {
            await _context.Notifications
                .Where(n => n.Id == notificationId).ExecuteUpdateAsync(n=>n.
                SetProperty(p=>p.IsDeleted,true));
           
        }

     

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(string userId, int pageNumber, int pageSize,string lan)
        {
            var notifications = await _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            var result = notifications.Select(n =>
            {
                var metadata = !string.IsNullOrEmpty(n.MetadataJson)
                    ? JsonConvert.DeserializeObject<Dictionary<string, string>>(n.MetadataJson)
                    : null;

                var (title, body) = NotificationTemplateEngine.Generate(n.Type, lan, metadata);

                return new NotificationResponseDto
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Type = n.Type,
                    ReferenceId = n.ReferenceId,
                    IsSeen = n.IsSeen,
                    CreatedAt = n.CreatedAt,
                    LastSeenAt = n.LastSeenAt,
                    ImageUrl = FileSetting.GetMediaUrl(n.ImageUrl, MediaType.userImage),
                    Title = title,
                    Body = body
                };
            }).ToList();
            return result;
        }

        public async Task MarkAsSeenAsync(int notificationId)
        {
             await _context.Notifications
                .Where(n => n.Id == notificationId && !n.IsSeen).ExecuteUpdateAsync(n=>n
                .SetProperty(p => p.IsSeen, true)
                .SetProperty(p => p.LastSeenAt, _dateTimeHelper.Now)
                );

           
        }

        public async Task MarkAllAsSeenByUserIdAsync(string userId)
        {
             await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsSeen).ExecuteUpdateAsync(n=>n
                .SetProperty(p => p.LastSeenAt, _dateTimeHelper.Now));
        }

        public async Task<int> CountNotificationByuserId(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsSeen && (n.LastSeenAt == null || n.CreatedAt > n.LastSeenAt))
                .CountAsync();
        }
    }
}
