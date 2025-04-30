using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TheLab.Services.Cache;

namespace TheLab.Filters
{
    public class CacheResultFilter : IResultFilter
    {
        private readonly ICacheService _cacheService;

        public CacheResultFilter(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var cacheKey = $"{context.HttpContext.Request.Path}";

            var cachedResult = _cacheService.Get(cacheKey);

            if (cachedResult != null)
            {
                context.Result = cachedResult as IActionResult ?? new ContentResult
                {
                    StatusCode = 404,
                    Content = "Cache result not valid"
                };
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is ObjectResult result)
            {
                var cacheKey = $"{context.HttpContext.Request.Path}";
                _cacheService.Set(cacheKey, result); 
            }
        }
    }
}
