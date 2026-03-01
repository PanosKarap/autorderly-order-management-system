using FirstProjectAPI.DTOs.Login;

namespace FirstProjectAPI.Services.Login
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}