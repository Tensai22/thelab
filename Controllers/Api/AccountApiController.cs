using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheLab.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

namespace TheLab.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly ILogger<AccountApiController> _logger;  

        public AccountApiController(AppDbContext context, TokenService tokenService, ILogger<AccountApiController> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger; 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            _logger.LogInformation("Login attempt for user: {Username}", model.Username);  

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for user: {Username}", model.Username);  
                return Unauthorized();
            }

            var token = await _tokenService.GenerateAccessToken(user);
            _logger.LogInformation("User {Username} successfully logged in.", model.Username);  

            return Ok(new { Token = token });
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var username = User.Identity?.Name;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            _logger.LogInformation("User {Username} accessed their profile.", username); 

            return Ok(new
            {
                Username = username,
                Email = email
            });
        }
    }
}
