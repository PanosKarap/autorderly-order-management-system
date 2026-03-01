using FirstProjectAPI.DTOs;
using FirstProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirstProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // Ζητάμε από το σύστημα να μας φέρει τον διαχειριστή χρηστών (Dependency Injection)
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("create-client")]
        [Authorize(Roles = "Admin")] // <--- ΜΟΝΟ Ο ADMIN ΠΕΡΝΑΕΙ ΑΠΟ ΕΔΩ!
        public async Task<IActionResult> CreateClient([FromBody] RegisterDto dto)
        {
            // 1. Δημιουργούμε τον νέο χρήστη/μαγαζί
            var newUser = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                StoreName = dto.StoreName
            };

            // 2. Τον αποθηκεύουμε στη βάση με τον κωδικό του
            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (result.Succeeded)
            {
                // 3. Του δίνουμε τον ρόλο του απλού μαγαζιού
                await _userManager.AddToRoleAsync(newUser, "StoreOwner");
                return Ok(new { Message = $"Το μαγαζί '{dto.StoreName}' δημιουργήθηκε επιτυχώς!" });
            }

            // Αν κάτι πάει λάθος (π.χ. αδύναμος κωδικός ή υπάρχει ήδη το email)
            return BadRequest(result.Errors);
        }
    }
}