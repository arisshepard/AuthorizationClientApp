using AuthorizationClientApp.Server.Data;
using AuthorizationClientApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationClientApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public LoginController(IConfiguration configuration,
                               SignInManager<IdentityUser> signInManager,
                               ApplicationDbContext context)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles (string ID)
        {
            var roles = await (from userRole in _context.UserRoles
                               join role in _context.Roles 
                               on userRole.RoleId equals role.Id
                               where userRole.UserId == ID
                               select role.Name)
                .ToListAsync();

            if (!roles.Any())
            {
                roles = new System.Collections.Generic.List<string>() { "Admin" };
            }

            return Ok(roles);
        } 

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, login.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));
            var expiry = DateTime.Now.AddSeconds(10);

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}