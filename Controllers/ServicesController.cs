using Microsoft.AspNetCore.Mvc;

namespace TheLab.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Testimonials() 
        {
            return View();
        }

    }
}
