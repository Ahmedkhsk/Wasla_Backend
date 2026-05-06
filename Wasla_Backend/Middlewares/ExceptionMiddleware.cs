namespace Wasla_Backend.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var lan = context.Request.Query["lan"].ToString();
            if (string.IsNullOrWhiteSpace(lan))
                lan = context.Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault() ?? "en";
            if (string.IsNullOrWhiteSpace(lan))
                lan = "en";

            HttpStatusCode statusCode;
            LocalizationKey key;
            object? data = null;

            switch (ex)
            {
                case BadRequestException bre:
                    statusCode = HttpStatusCode.BadRequest;
                    key = bre.Key;
                    break;

                case NotFoundException nfe:
                    statusCode = HttpStatusCode.NotFound;
                    key = nfe.Key;
                    break;

                case UnauthorizedException ue:
                    statusCode = HttpStatusCode.Unauthorized;
                    key = ue.Key;
                    break;

                case ForbiddenException fe:
                    statusCode = HttpStatusCode.Forbidden;
                    key = fe.Key;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    key = LocalizationKey.ServerError;

                    data = new
                    {
                        error = ex.Message,
                        stackTrace = ex.StackTrace
                    };
                    break;
            }
            context.Response.StatusCode = (int)statusCode;

            var response = ResponseHelper.Fail(key, lan, data);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}