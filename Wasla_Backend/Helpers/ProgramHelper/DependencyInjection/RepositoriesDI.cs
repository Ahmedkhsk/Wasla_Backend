namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class RepositoriesDI
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IResidentRepository, ResidentRepository>();
            services.AddScoped<IResidentIdentityRepository, ResidentIdentityRepository>();
            services.AddScoped<IDoctorServiceRepository, DoctorServiceRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IFavouriteRepository, FavouriteRepository>();
            services.AddScoped<IGymRepository, GymRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IGymBookingRepository, GymBookingRepository>();
            services.AddScoped<IUserEventRepository, UserEventRepository>();

            return services;
        }
    }
}
