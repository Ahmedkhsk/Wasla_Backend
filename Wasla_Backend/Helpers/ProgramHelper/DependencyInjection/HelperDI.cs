namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class HelperDI
    {
        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddTransient<IEmailSenderHelper,EmailSenderHelper>();
            services.AddSingleton<IDateTimeHelper,DateTimeHelper>();
            services.AddSingleton<ICacheManager,CacheManager>();
            services.AddScoped<FileValidator>();
            services.AddSingleton<UserConnectionHelper>();
            services.AddScoped<PaymobPaymentStrategy>();
            services.AddScoped<CashPaymentStrategy>();
            services.AddScoped<IPaymentStrategyFactory, PaymentStrategyFactory>();

            return services;
        }
    }
}
