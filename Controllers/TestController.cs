using Microsoft.AspNetCore.Mvc;

namespace TheLab.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("server-error")]
        public IActionResult TriggerServerError()
        {
            throw new Exception("Тестовая ошибка сервера!");
        }

        [HttpGet("not-found")]
        public IActionResult TriggerNotFound()
        {
            throw new FileNotFoundException("Ресурс не найден");
        }

        [HttpGet("unauthorized")]
        public IActionResult TriggerUnauthorized()
        {
            throw new UnauthorizedAccessException("Требуется авторизация");
        }
    }
}
