namespace Wasla_Backend.Middlewares
{
    public class ClientRequestInfo
    {
        
        public Queue<DateTime> RequestTimes { get; } = new();
        public DateTime LastRequestTime { get; set; } = DateTime.UtcNow;
    }

}
