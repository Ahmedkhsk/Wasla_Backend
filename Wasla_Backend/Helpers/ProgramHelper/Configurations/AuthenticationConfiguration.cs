using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class AuthenticationConfiguration
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = config["JWT:Issuer"],
                ValidAudience = config["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["JWT:SecretKey"]))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    Console.WriteLine("===== OnMessageReceived =====");
                    Console.WriteLine("Request Path: " + path);
                    Console.WriteLine("Access Token from Query: " + accessToken);

                    if (string.IsNullOrEmpty(accessToken) && context.Request.Headers.ContainsKey("Authorization"))
                    {
                        accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        Console.WriteLine("Access Token from Header: " + accessToken);
                    }

                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/bookingHub") ||
                         path.StartsWithSegments("/serviceHub") ||
                         path.StartsWithSegments("/rideHub") ||
                         path.StartsWithSegments("/reviewHub") ||
                         path.StartsWithSegments("/chatHub")))
                    {
                        context.Token = accessToken;
                        Console.WriteLine("Token assigned to context.Token ✔");
                    }

                    return Task.CompletedTask;
                },

                
            };
        });

        return services;
    }
}