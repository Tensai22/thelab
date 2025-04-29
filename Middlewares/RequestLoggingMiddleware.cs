namespace TheLab.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            await _next(context); // передача запроса дальше

            var duration = DateTime.UtcNow - startTime;
            _logger.LogInformation(
                $"Request: {context.Request.Method} {context.Request.Path} | " +
                $"Status: {context.Response.StatusCode} | " +
                $"Duration: {duration.TotalMilliseconds} ms");
        }
    }

    // Расширение для удобного подключения
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
