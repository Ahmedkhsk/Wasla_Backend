using System.Threading;

namespace Wasla_Backend.Helpers.BackgroundServiceHelper
{
    public class BookingStatusUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BookingStatusUpdaterService> _logger;
        private readonly IHubContext<BookingHub> _hub;

        private static readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public BookingStatusUpdaterService(
            IServiceScopeFactory scopeFactory,
            ILogger<BookingStatusUpdaterService> logger,
            IHubContext<BookingHub> hub)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hub = hub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessBookings(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "BookingStatusUpdaterService failed.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task ProcessBookings(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Context>();

            var nowUtc = DateTime.UtcNow;

            var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);

            var upcomingBookings = await db.Booking
                .Include(b => b.serviceDay)
                .Where(b =>
                    b.bookingStatus == BookingStatus.upcoming &&
                    b.bookingDate == todayUtc
                )
                .ToListAsync(stoppingToken);

            foreach (var booking in upcomingBookings)
            {
                var startString = booking.newStart ?? booking.serviceDay.start;
                var endString = booking.newEnd ?? booking.serviceDay.end;

                if (!TimeOnly.TryParse(startString, out var startTime)) continue;
                if (!TimeOnly.TryParse(endString, out var endTime)) continue;

                var startDateTime = booking.bookingDate.ToDateTime(startTime);
                var endDateTime = booking.bookingDate.ToDateTime(endTime);

                if (endTime <= startTime)
                    endDateTime = endDateTime.AddDays(1);

                if (booking.bookingStatus == BookingStatus.upcoming && endDateTime <= nowUtc)
                {
                    booking.bookingStatus = BookingStatus.completed;
                    booking.serviceDay.isBooking = false;

                    var hubData = new BookHubData
                    {
                        serviceId = booking.serviceDayId,
                        residentId = booking.userId,
                        serviceProviderId = booking.serviceProviderId
                    };

                    await NotifyUsers(hubData, booking, stoppingToken);
                }
            }

            await db.SaveChangesAsync(stoppingToken);
        }

        private async Task NotifyUsers(BookHubData data, Booking booking, CancellationToken cancellationToken)
        {
            await _hub.Clients
             .User(booking.userId)
             .SendAsync("BookingCompleted", data, cancellationToken);

            await _hub.Clients
                .User(booking.serviceProviderId)
                .SendAsync("BookingCompleted", data, cancellationToken);
        }
    }
}
