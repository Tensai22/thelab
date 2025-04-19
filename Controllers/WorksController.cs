using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TheLab.Controllers
{
    public class WorksController : Controller
    {
        public IActionResult Works1()
        {
            return View();
        }

        public IActionResult Works2()
        {
            return View();
        }
        public IActionResult Blog1()
        {
            return View();
        }
    }
}