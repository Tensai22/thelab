using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TheLab.Models;


namespace TheLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Models.Message userMessage)
        {
            _logger.LogInformation("User {name} trying to send form to email {email}", userMessage.Name, userMessage.Email);

            EmailSender sender = new EmailSender();

            _logger.LogInformation("Starting data validation");
            if (string.IsNullOrWhiteSpace(userMessage.Name))
            {
                ModelState.AddModelError("name", "Name field is required to fill");
            }
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Form sent successfully");
                return RedirectToAction("FAQ");
            }
            else
            {
                _logger.LogError("{name}, errors occured while sending form", userMessage.Name);
                return View(userMessage);
            }
        }


        public IActionResult AboutUs() 
        {
            return View();
        }
        public IActionResult FAQ() 
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
