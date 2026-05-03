namespace Wasla_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                            .AddIdentityServices()
                            .AddCorsConfig()
                            .AddLocalizationConfig()
                            .AddSwaggerConfig()
                            .AddRepositories()
                            .AddMLServices()
                            .AddApplicationServices()
                            .AddHelpers()
                            .AddEndpointsApiExplorer()
                            .AddAutoMapper(Assembly.GetExecutingAssembly())
                            .AddJwtAuthentication(builder.Configuration)
                            .AddDatabase(builder.Configuration)
                            .AddHangfireServices(builder.Configuration)
                            .AddAppOptions(builder.Configuration)
                            .AddControllers();
       

            builder.Services.AddSignalR();
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();

            var app = builder.Build();

            app.UseApplicationPipeline();


            app.Run();
        }
    }
}