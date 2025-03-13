using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheLab.Models;

namespace TheLab.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("User accessed login page.");
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login attempt failed: Invalid model state.");
                return View(model);
            }

            var user = UserStore.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user == default)
            {
                _logger.LogWarning("Failed login attempt for username: {Username}", model.Username);
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            _logger.LogInformation("User {Username} authenticated successfully with role {Role}", user.Username, user.Role);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(5)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            _logger.LogInformation("User {Username} signed in successfully. Cookie expires at {ExpiresUtc}", user.Username, authProperties.ExpiresUtc);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name;
            _logger.LogInformation("User {Username} is logging out.", username);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("User {Username} has logged out.", username);
            return RedirectToAction("Login");
        }
    }
}
