using System.Security.Claims;

namespace TheLab.Middlewares
{

    public class AuthenticatedRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticatedRequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var username = user.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
                var method = context.Request.Method;
                var path = context.Request.Path;

                Console.WriteLine($"[{DateTime.Now}] {username} -> {method} {path}");
            }

            await _next(context);
        }
    }

}
