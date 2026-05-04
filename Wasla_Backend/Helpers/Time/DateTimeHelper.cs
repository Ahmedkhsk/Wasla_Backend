namespace Wasla_Backend.Helpers.Time
{
    public class DateTimeHelper : IDateTimeHelper
    {
        private readonly TimeZoneInfo _timeZone;

        public DateTimeHelper(IOptions<TimeZoneSettings> options)
        {
            _timeZone = TimeZoneInfo.FindSystemTimeZoneById(options.Value.Default);
        }

        public DateTime Now =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);

        public TimeSpan CalculateDelay(DateOnly date, TimeOnly time)
        {
            var now = Now;

            var nowDate = DateOnly.FromDateTime(now);
            var nowTime = TimeOnly.FromDateTime(now);

            var daysDiff = date.DayNumber - nowDate.DayNumber;
            var timeDiff = time - nowTime;

            var delay = TimeSpan.FromDays(daysDiff) + timeDiff;

            if (delay <= TimeSpan.Zero)
            {
                delay = TimeSpan.FromSeconds(10);
            }

            return delay;
        }
    }

}
