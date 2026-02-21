namespace Wasla_Backend.Helpers.ProgramHelper.Configurations
{
    public static class DbConfiguration
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddHangfire(x =>
                x.UseSqlServerStorage(config.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer();
            return services;
        }
    }
}
