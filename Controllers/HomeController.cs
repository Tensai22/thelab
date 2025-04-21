using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization; // Добавить для работы с локализацией
using System.Diagnostics;
using System.Globalization; // Для работы с CultureInfo
using TheLab.Models;

namespace TheLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer; // Зависимость для локализации

        // Конструктор для внедрения зависимостей
        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer; // Инициализация локализатора
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
                return RedirectToAction("Contact");
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

        

        [HttpPost]
        public JsonResult ChangeCulture(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),

                new CookieOptions { Expires = DateTime.Now.AddMonths(1) }
                );

            return Json(culture);
        }
    }
}
