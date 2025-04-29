using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheLab.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TheLab.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly TokenService _tokenService;

        public AccountController(
            AppDbContext context,
            ILogger<AccountController> logger,
            TokenService tokenService)
        {
            _context = context;
            _logger = logger;
            _tokenService = tokenService;
        }

        #region MVC Actions (Cookie Auth)
        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                return View(model);
            }

            await SignInUser(user);
            _logger.LogInformation("User {Username} logged in", user.Username);

            return LocalRedirect(returnUrl ?? "/");
        }

        [HttpGet("Register")]
        public IActionResult Register() => View();

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username taken");
                return View(model);
            }

            var user = new AppUser
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(AppUser user, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = rememberMe });
        }
        #endregion

        #region API Actions (JWT Auth)
        [Route("api/[controller]")]
        [ApiController]
        public class AccountApiController : ControllerBase
        {
            private readonly AppDbContext _context;
            private readonly TokenService _tokenService;

            public AccountApiController(AppDbContext context, TokenService tokenService)
            {
                _context = context;
                _tokenService = tokenService;
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginViewModel model)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                    return Unauthorized();

                var token = await _tokenService.GenerateAccessToken(user);
                return Ok(new { Token = token });
            }

            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            [HttpGet("profile")]
            public IActionResult GetProfile()
            {
                return Ok(new
                {
                    Username = User.Identity?.Name,
                    Email = User.FindFirst(ClaimTypes.Email)?.Value
                });
            }
        }
        #endregion
    }
}