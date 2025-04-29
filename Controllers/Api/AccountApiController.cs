using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheLab.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TheLab.Controllers.Api
{
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
        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Test exception for middleware.");
        }

    }
}
