using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TheLab.Filters
{
    public class CatchError : IExceptionFilter
    {
        private readonly ILogger<CatchError> _logger;
        public CatchError(ILogger<CatchError> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogWarning(context.Exception.Message);

            context.Result = new RedirectToActionResult("Error", "Home",
                new { message = context.Exception.Message });
        }
    }
}
