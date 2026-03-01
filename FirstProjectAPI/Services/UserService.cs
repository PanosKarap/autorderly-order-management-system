using FirstProjectAPI.DTOs.Register;
using FirstProjectAPI.Interfaces;
using FirstProjectAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FirstProjectAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var newUser = new ApplicationUser { UserName = dto.Email, Email = dto.Email, StoreName = dto.StoreName };
            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "StoreOwner");
            }
            return result;
        }

        public async Task<IEnumerable<RegisterResponseDto>> GetAllStoresAsync()
        {
            var stores = await _userManager.GetUsersInRoleAsync("StoreOwner");
            return stores.Select(s => new RegisterResponseDto
            {
                Email = s.Email!,
                StoreName = s.StoreName
            });
        }

        public async Task<IdentityResult> UpdateStoreAsync(RegisterResponseDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Το μαγαζί δεν βρέθηκε." });

            user.StoreName = dto.StoreName;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteStoreAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Το μαγαζί δεν βρέθηκε." });

            return await _userManager.DeleteAsync(user);
        }
    }
}