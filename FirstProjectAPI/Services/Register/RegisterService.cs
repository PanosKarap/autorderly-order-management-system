using FirstProjectAPI.DTOs.Register;
using FirstProjectAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace FirstProjectAPI.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // Ο Constructor φέρνει τον UserManager
        public RegisterService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var newUser = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                StoreName = dto.StoreName
            };

            // Το CreateAsync επιστρέφει ένα αντικείμενο τύπου IdentityResult
            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "StoreOwner");
            }

            return result; // Τώρα αυτό το result έχει μέσα το .Succeeded και το .Errors
        }
    }
}