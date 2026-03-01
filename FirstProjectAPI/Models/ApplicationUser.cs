using Microsoft.AspNetCore.Identity;

namespace FirstProjectAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Εδώ μπορείς να προσθέσεις extra πεδία για το μαγαζί
        public string StoreName { get; set; }
    }
}