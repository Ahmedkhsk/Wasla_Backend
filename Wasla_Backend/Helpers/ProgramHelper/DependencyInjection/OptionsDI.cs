namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class OptionsDI
    {
        public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtSettings>(config.GetSection("Jwt"));
            services.Configure<EmailSettings>(config.GetSection("Email"));
            services.Configure<RateLimitSettings>(config.GetSection("RateLimitSettings"));
            services.Configure<TimeZoneSettings>(config.GetSection("TimeZones"));
            services.AddMemoryCache();

            return services;
        }
    }
}
