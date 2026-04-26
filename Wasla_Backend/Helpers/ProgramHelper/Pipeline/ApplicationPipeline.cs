namespace Wasla_Backend.Helpers.ProgramHelper.Pipeline
{
    public static class ApplicationPipeline
    {
        public static WebApplication UseApplicationPipeline(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wasla API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHangfireDashboard("/hangfire");

            app.MapHub<BookingHub>("/bookingHub");
            app.MapHub<ServiceHub>("/serviceHub");
            app.MapHub<ReviewHub>("/reviewHub");
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<RideHub>("/rideHub");
            app.MapHub<OrderHub>("/orderHub");

            app.MapControllers();

            return app;
        }
    }
}
