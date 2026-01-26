namespace Wasla_Backend.Helpers.Time
{
    public class DateTimeHelper
    {
        private readonly TimeZoneInfo _timeZone;

        public DateTimeHelper(IOptions<TimeZoneSettings> options)
        {
            _timeZone = TimeZoneInfo.FindSystemTimeZoneById(options.Value.Default);
        }

        public DateTime Now =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
    }

}
