using FirstProjectAPI.DTOs.Login;

namespace FirstProjectAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}