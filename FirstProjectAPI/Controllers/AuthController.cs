using FirstProjectAPI.DTOs;
using FirstProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // 1. Έλεγχος αν υπάρχει ο χρήστης
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                // 2. Δημιουργία των Claims (πληροφορίες μέσα στο token)
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // Εδώ είναι το UserId!
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                // --- ΝΕΟΣ ΚΩΔΙΚΑΣ: Προσθήκη των Roles στο Token! ---
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                // ----------------------------------------------------

                // 3. Δημιουργία του Token
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3), // Το token λήγει σε 3 ώρες
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                // 4. Επιστροφή του Token στη React
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    storeName = user.StoreName // Στέλνουμε και το όνομα του μαγαζιού για το UI
                });
            }

            return Unauthorized(new { Message = "Λάθος Email ή Κωδικός." });
        }
    }
}