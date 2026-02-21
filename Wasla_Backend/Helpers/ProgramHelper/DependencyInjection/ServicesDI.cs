namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class ServicesDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IResidentService, ResidentService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IGymService, GymService>();
            services.AddScoped<IPaymentService, PaymobService>();
            services.AddScoped<IDoctorBookService, DoctorBookService>();
            services.AddScoped<IFavouriteService, FavouriteService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IGymBookingService, GymBookingService>();
            services.AddScoped<IDoctorServiceService, DoctorServiceService>();
            services.AddScoped<IUserEventService, UserEventService>();

            return services;
        }
    }
}
