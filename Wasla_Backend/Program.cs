namespace Wasla_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddHostedService<ExpiredEmailVerificationCleaner>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IResidentRepository, ResidentRepository>();
            builder.Services.AddScoped<IResidentIdentityRepository, ResidentIdentityRepository>();
            builder.Services.AddScoped<IDoctorServiceRepository, DoctorServiceRepository>();
            builder.Services.AddTransient<EmailSenderHelper>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IResidentService, ResidentService>();
            builder.Services.AddScoped<IDoctorServiceService, DoctorServiceService>();
            builder.Services.AddScoped<TokenHelper>();

            builder.Services.AddScoped<IUserFactory, UserFactory>();

          builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };
            });

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en", "ar" };
                options.SetDefaultCulture("en");
                options.AddSupportedCultures(supportedCultures);
                options.AddSupportedUICultures(supportedCultures);
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");

            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.RoutePrefix = string.Empty; 
            });

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
