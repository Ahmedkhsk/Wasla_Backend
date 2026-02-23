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
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);
            if (notification == null)
                return;
            notification.IsDeleted = true;


            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationByUserIdAfterLastSeenAsync(string userId, int pageNumber, int pageSize)
        {
            return await _context.Notifications
                 .AsNoTracking()
                 .Where(n => n.UserId == userId && (n.LastSeenAt == null || n.CreatedAt > n.LastSeenAt))
                 .OrderByDescending(n => n.CreatedAt)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .Select(n => new NotificationResponseDto
                 {
                     Id = n.Id,
                     UserId = n.UserId,
                     Type = n.Type,
                     ReferenceId = n.ReferenceId,
                     Title = n.Title,
                     Body = n.Body,
                     IsSeen = n.IsSeen,
                     CreatedAt = n.CreatedAt,
                     LastSeenAt = n.LastSeenAt
                 })
                 .ToListAsync();
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(string userId, int pageNumber, int pageSize)
        {
            return await _context.Notifications
                 .AsNoTracking()
                 .Where(n => n.UserId == userId)
                 .OrderByDescending(n => n.CreatedAt)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .Select(n => new NotificationResponseDto
                 {
                     Id = n.Id,
                     UserId = n.UserId,
                     Type = n.Type,
                     ReferenceId = n.ReferenceId,
                     Title = n.Title,
                     Body = n.Body,
                     IsSeen = n.IsSeen,
                     CreatedAt = n.CreatedAt,
                     LastSeenAt = n.LastSeenAt
                 })
                 .ToListAsync();
        }

        public async Task MarkAsSeenAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && !n.IsSeen);

            if (notification == null) return;

            notification.IsSeen = true;
            notification.LastSeenAt = _dateTimeHelper.Now;

            await _context.SaveChangesAsync();
        }

        public async Task MarkAllAsSeenByUserIdAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsSeen)
                .ToListAsync();

            foreach (var n in notifications)
            {
                n.IsSeen = true;
                n.LastSeenAt = _dateTimeHelper.Now;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> CountNotificationByuserId(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsSeen)
                .CountAsync();
        }
    }
}
