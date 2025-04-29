using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TheLab.Filters
{
    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string _secretKey = "my_secret_key_123";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasHeader = context.HttpContext.Request.Headers.TryGetValue("X-Secret-Key", out var providedKey);

            if (!hasHeader || providedKey != _secretKey)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = "Доступ запрещен. Неверный ключ."
                };
            }
        }
    }
}


