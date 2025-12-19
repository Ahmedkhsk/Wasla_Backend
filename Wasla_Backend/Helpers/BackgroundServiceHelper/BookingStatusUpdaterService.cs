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

            var upcomingBookings = await db.Booking
                .Include(b => b.serviceDay)
                .Where(b => b.bookingStatus == BookingStatus.upcoming)
                .ToListAsync(stoppingToken);

            foreach (var booking in upcomingBookings)
            {
                if (booking.bookingStatus != BookingStatus.upcoming)
                    continue;

                if (booking.serviceDay == null)
                    continue;

                var endString = !string.IsNullOrWhiteSpace(booking.newEnd)
                    ? booking.newEnd
                    : booking.serviceDay.end;

                if (!TimeOnly.TryParse(endString, out var endTime))
                {
                    _logger.LogWarning(
                        $"Invalid end time format for booking {booking.Id}");
                    continue;
                }

                var bookingEndUtc =
                    booking.bookingDate.ToDateTime(endTime, DateTimeKind.Utc);

                if (bookingEndUtc > nowUtc)
                    continue;

                booking.bookingStatus = BookingStatus.completed;
                booking.serviceDay.isBooking = false;

                var hubData = new BookHubData
                {
                    serviceId = booking.serviceDayId,
                    residentId = booking.userId,
                    serviceProviderId = booking.serviceProviderId
                };

                await NotifyUsers(hubData, booking);
            }

            await db.SaveChangesAsync(stoppingToken);
        }

        private async Task NotifyUsers(BookHubData data, Booking booking)
        {
            await _hub.Clients
                .User(booking.userId)
                .SendAsync("BookingCompleted", data);

            await _hub.Clients
                .User(booking.serviceProviderId)
                .SendAsync("BookingCompleted", data);
        }
    }
}
