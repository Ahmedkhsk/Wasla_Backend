namespace Wasla_Backend.Helpers.ProgramHelper.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();
            return app;
        }
    }
}
