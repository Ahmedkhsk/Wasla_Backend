namespace Wasla_Backend.Helpers.BackgroundServiceHelper
{
    public class BookingStatusUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BookingStatusUpdaterService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public BookingStatusUpdaterService(
            IServiceScopeFactory scopeFactory,
            ILogger<BookingStatusUpdaterService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<Context>();

                    var now = DateTime.Now;

                    var upcomingBookings = await db.Booking
                        .Include(b => b.serviceDay)
                        .Where(b => b.bookingStatus == BookingStatus.upcoming)
                        .ToListAsync(stoppingToken);

                    var toComplete = upcomingBookings
                        .Where(b =>
                        {
                            var endString = !string.IsNullOrWhiteSpace(b.newEnd)
                                ? b.newEnd
                                : b.serviceDay.end;

                            var endTime = TimeOnly.Parse(endString);
                            var bookingDateTime = b.bookingDate.ToDateTime(endTime);
                            return bookingDateTime <= now;
                        })
                        .ToList();

                    if (toComplete.Any())
                    {
                        foreach (var booking in toComplete)
                        {
                            booking.bookingStatus = BookingStatus.completed;

                            if (booking.serviceDay != null)
                                booking.serviceDay.isBooking = false;
                        }

                        await db.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during BookingStatusUpdaterService execution.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
