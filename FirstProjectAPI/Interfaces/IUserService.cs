using FirstProjectAPI.DTOs.Register;
using Microsoft.AspNetCore.Identity;

namespace FirstProjectAPI.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
        Task<IEnumerable<RegisterResponseDto>> GetAllStoresAsync();
        Task<IdentityResult> UpdateStoreAsync(RegisterResponseDto dto);
        Task<IdentityResult> DeleteStoreAsync(string email);
    }
}