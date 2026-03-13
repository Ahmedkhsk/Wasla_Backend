namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class HelperDI
    {
        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddScoped<TokenHelper>();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddTransient<EmailSenderHelper>();
            services.AddSingleton<DateTimeHelper>();
            services.AddSingleton<CacheManager>();
            services.AddScoped<FileValidator>();

            return services;
        }
    }
}
