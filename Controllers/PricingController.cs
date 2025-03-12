using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TheLab.Controllers
{
    public class PricingController : Controller
    {
        public IActionResult Pricing()
        {
            return View();
        }
    }
}