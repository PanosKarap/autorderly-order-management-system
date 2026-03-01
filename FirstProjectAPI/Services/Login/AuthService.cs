using FirstProjectAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirstProjectAPI.DTOs.Login;

namespace FirstProjectAPI.Services.Login
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        // Χρειαζόμαστε οπωσδήποτε τον Constructor για να "γεμίσουν" τα _userManager και _configuration
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            // 1. Βρίσκουμε τον χρήστη ΜΙΑ φορά
            var user = await _userManager.FindByEmailAsync(dto.Email);

            // 2. Έλεγχος αν υπάρχει και αν ο κωδικός είναι σωστός
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return null;
            }

            // 3. Φτιάχνουμε τα Claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 4. Δημιουργία Token
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // 5. Επιστρέφουμε το πλήρες πακέτο DTO
            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                StoreName = user.StoreName,
                Expiration = token.ValidTo
            };
        }
    }
}