namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class ServicesDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<IUserService>()
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
             );

            return services;
        }
    }
}
