using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wasla_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Wasla_Backend.BackgroundServices
{
    public class ExpiredEmailVerificationCleaner : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExpiredEmailVerificationCleaner> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public ExpiredEmailVerificationCleaner(IServiceScopeFactory scopeFactory, ILogger<ExpiredEmailVerificationCleaner> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExpiredEmailVerificationCleaner started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<Context>();

                        var expired = await dbContext.EmailVerifications
                            .Where(e => e.ExpiresAt <= DateTime.UtcNow)
                            .ToListAsync(stoppingToken);

                        if (expired.Any())
                        {
                            dbContext.EmailVerifications.RemoveRange(expired);
                            await dbContext.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation("Deleted {count} expired email verifications at {time}.", expired.Count, DateTime.UtcNow);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while cleaning expired email verifications.");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("ExpiredEmailVerificationCleaner stopped.");
        }
    }
}
