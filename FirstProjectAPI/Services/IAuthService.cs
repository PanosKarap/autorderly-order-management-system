using FirstProjectAPI.DTOs;

namespace FirstProjectAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}