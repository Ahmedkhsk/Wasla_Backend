namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class ServicesDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                 .FromAssemblies(
                     typeof(IUserService).Assembly
                 )
                 .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
                 .AsImplementedInterfaces()
                 .WithScopedLifetime()
             );
            services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();


            return services;
        }
    }
}
