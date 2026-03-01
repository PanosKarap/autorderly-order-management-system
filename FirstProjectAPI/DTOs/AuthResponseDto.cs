namespace FirstProjectAPI.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}