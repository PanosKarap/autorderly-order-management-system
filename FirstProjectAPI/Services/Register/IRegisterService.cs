using Microsoft.AspNetCore.Identity; // Μην ξεχάσεις αυτό το using
using FirstProjectAPI.DTOs.Register;

namespace FirstProjectAPI.Services.Register
{
    public interface IRegisterService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
    }
}