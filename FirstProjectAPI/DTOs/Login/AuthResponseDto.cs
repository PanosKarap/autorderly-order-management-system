namespace FirstProjectAPI.DTOs.Login
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}