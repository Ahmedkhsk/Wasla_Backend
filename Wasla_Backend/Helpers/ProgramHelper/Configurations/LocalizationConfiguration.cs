namespace Wasla_Backend.Helpers.ProgramHelper.Configurations
{
    public static class LocalizationConfiguration
    {
        public static IServiceCollection AddLocalizationConfig(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en", "ar" };

                options.SetDefaultCulture("en");
                options.AddSupportedCultures(supportedCultures);
                options.AddSupportedUICultures(supportedCultures);
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            return services;
        }
    }
}
