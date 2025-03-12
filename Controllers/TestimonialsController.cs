using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TheLab.Controllers
{
    public class TestimonialsController : Controller
    {
        public IActionResult Testimonials()
        {
            return View();
        }
    }
}