namespace Wasla_Backend.Middlewares
{
    public class RateLimitSettings
    {
        public bool EnableRateLimiting { get; set; }
        public int RequestLimit { get; set; }
        public int TimeWindowInSeconds { get; set; }
        public List<string> WhitelistedIPs { get; set; } = new();
    }

}
