using Microsoft.AspNetCore.Mvc;
using TheLab.Models;

namespace TheLab.Controllers
{
    public class SharedController : Controller
    {
        public IActionResult Testreg()
        {
            return View();
        }

    }
}