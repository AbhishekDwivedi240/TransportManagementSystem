using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TransportManagementSystem.Data;
using TransportManagementSystem.Model;

namespace TransportManagementSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration con;
        private ApDb db;

        public AuthController(IConfiguration configuration, ApDb apDb)
        {
            con = configuration;
            db = apDb;
        }

        public async Task<IActionResult>Register(User user)
        {

        }



        [HttpPost("Login")]
        public async Task<IActionResult> Get(User user)
        {
            // Check user from database
            var ur = await db.Users.FirstOrDefaultAsync(x =>
                x.UserName == user.UserName &&
                x.Password == user.Password);

            // Invalid user
            if (ur == null)
            {
                return Unauthorized();
            }

            // Claims
            var Claim = new[]
            {
                new Claim(ClaimTypes.Name, ur.UserName),
                new Claim(ClaimTypes.Role, ur.role),
                new Claim("UserId", ur.Id.ToString())
            };

            // Secret Key
            var Key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(con["JWT:Key"]));

            // Signing Credentials
            var creds = new SigningCredentials(
                Key,
                SecurityAlgorithms.HmacSha256);

            // Create Token
            var token = new JwtSecurityToken(
                issuer: con["JWT:Issuer"],
                audience: con["JWT:Audience"],
                claims: Claim,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            // Return Token
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}