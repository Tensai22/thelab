using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

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
