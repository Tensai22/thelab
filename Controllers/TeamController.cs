using Microsoft.AspNetCore.Mvc;

namespace TheLab.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult OurTeam()
        {
            return View();
        }
    }
}
