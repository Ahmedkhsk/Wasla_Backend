namespace Wasla_Backend.Helpers.Time
{
    public interface IDateTimeHelper
    {
        DateTime Now { get; }
        public TimeSpan CalculateDelay(DateOnly date, TimeOnly time);

    }
}
