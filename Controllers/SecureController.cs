using Microsoft.AspNetCore.Mvc;
using TheLab.Filters;

namespace TheLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecureController : ControllerBase
    {
        [HttpGet]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        public IActionResult SecretData()
        {
            return Ok("Секретные данные доступны");
        }
    }

}
