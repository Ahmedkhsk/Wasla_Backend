namespace Wasla_Backend.Helpers.ProgramHelper.Configurations
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.SetIsOriginAllowed(origin =>
                    {
                        if (string.IsNullOrWhiteSpace(origin))
                            return false;

                        // Localhost
                        if (origin.StartsWith("http://localhost") ||
                            origin.StartsWith("http://127.0.0.1"))
                            return true;

                        // Frontend Hosts
                        var allowedHosts = new[]
                        {
                        ".vercel.app",
                        ".netlify.app",
                        ".firebaseapp.com"
                    };

                        return allowedHosts.Any(h => origin.Contains(h));
                    })
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            return services;
        }
    }
}
