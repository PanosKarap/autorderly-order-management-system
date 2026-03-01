namespace FirstProjectAPI.DTOs
{
    public class LoginDto
    {
        // Χρησιμοποιούμε [Required] για να είμαστε σίγουροι ότι το API 
        // θα απορρίψει το αίτημα αν λείπει κάποιο πεδίο
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}