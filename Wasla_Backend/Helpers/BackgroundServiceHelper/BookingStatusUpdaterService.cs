namespace Wasla_Backend.Helpers.BackgroundServiceHelper
{
    public class BookingStatusUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BookingStatusUpdaterService> _logger;
        private readonly IHubContext<BookingHub> _hub;
        private readonly TimeZoneInfo _cairoTimeZone;

        private static readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public BookingStatusUpdaterService(
            IServiceScopeFactory scopeFactory,
            ILogger<BookingStatusUpdaterService> logger,
            IHubContext<BookingHub> hub,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hub = hub;

            var timeZoneId = configuration["TimeZones:Default"];

            if (string.IsNullOrWhiteSpace(timeZoneId))
                throw new BadRequestException("TimeZoneNotConfigured");

            _cairoTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
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
                    _logger.LogError(ex, "BookingStatusUpdaterIterationFailed");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task ProcessBookings(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Context>();

            var nowCairo = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                _cairoTimeZone
            );

            var todayCairo = DateOnly.FromDateTime(nowCairo);
            var yesterdayCairo = todayCairo.AddDays(-1);

            var upcomingBookings = await db.Booking
                .Include(b => b.serviceDay)
                .Where(b =>
                    b.bookingStatus == BookingStatus.upcoming &&
                    (b.bookingDate == todayCairo || b.bookingDate == yesterdayCairo)
                )
                .ToListAsync(stoppingToken);

            var completedBookings = new List<(Booking booking, BookHubData hubData)>();

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

                if (endDateTime <= nowCairo)
                {
                    booking.bookingStatus = BookingStatus.completed;
                    booking.serviceDay.isBooking = false;

                    completedBookings.Add((
                        booking,
                        new BookHubData
                        {
                            serviceId = booking.serviceDayId,
                            residentId = booking.userId,
                            serviceProviderId = booking.serviceProviderId
                        }
                    ));
                }
            }

            if (completedBookings.Any())
            {
                await db.SaveChangesAsync(stoppingToken);

                foreach (var item in completedBookings)
                {
                    await NotifyUsers(item.hubData, item.booking, stoppingToken);
                }
            }
        }

        private async Task NotifyUsers(
            BookHubData data,
            Booking booking,
            CancellationToken cancellationToken)
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
