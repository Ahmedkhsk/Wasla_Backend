using System.Collections.Concurrent;

namespace Wasla_Backend.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimitSettings _settings;

        private static readonly ConcurrentDictionary<string, ClientRequestInfo> _clients = new();

        public RateLimitingMiddleware(
            RequestDelegate next,
            IOptions<RateLimitSettings> settings)
        {
            _next = next;
            _settings = settings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_settings.EnableRateLimiting)
            {
                await _next(context);
                return;
            }

            if (IsHubRequest(context))
            {
                await _next(context);
                return;
            }

            var clientKey = GetClientKey(context);
            if (string.IsNullOrEmpty(clientKey))
            {
                await _next(context);
                return;
            }

            if (_settings.WhitelistedIPs.Contains(clientKey))
            {
                await _next(context);
                return;
            }

            var clientInfo = _clients.GetOrAdd(clientKey, _ => new ClientRequestInfo());

            bool blocked;

            lock (clientInfo)
            {
                var now = DateTime.UtcNow;
                var cutoff = now.AddSeconds(-_settings.TimeWindowInSeconds);

                while (clientInfo.RequestTimes.Count > 0 &&
                       clientInfo.RequestTimes.Peek() < cutoff)
                {
                    clientInfo.RequestTimes.Dequeue();
                }

                if (clientInfo.RequestTimes.Count >= _settings.RequestLimit)
                {
                    blocked = true;
                }
                else
                {
                    clientInfo.RequestTimes.Enqueue(now);
                    clientInfo.LastRequestTime = now;
                    blocked = false;
                }
            }

            if (blocked)
            {
                await WriteRateLimitResponse(context);
                return;
            }

            CleanupIfNeeded();

            await _next(context);
        }


        private bool IsHubRequest(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            return path is not null &&
                   (path.StartsWith("/bookingHub") ||
                    path.StartsWith("/serviceHub") ||
                    path.StartsWith("/reviewHub"));
        }

        private string GetClientKey(HttpContext context)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                return $"user:{context.User.Identity.Name}";
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }

        private async Task WriteRateLimitResponse(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";

            var lan = context.Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";

            var response = ResponseHelper.Fail(LocalizationKey.TooManyRequests, lan, new
            {
                retryAfterSeconds = _settings.TimeWindowInSeconds
            });

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }


        private void CleanupIfNeeded()
        {
            if (_clients.Count < 1000) return;

            var cutoff = DateTime.UtcNow
                .AddSeconds(-_settings.TimeWindowInSeconds * 2);

            foreach (var client in _clients)
            {
                if (client.Value.LastRequestTime < cutoff)
                {
                    _clients.TryRemove(client.Key, out _);
                }
            }
        }
    }

}
