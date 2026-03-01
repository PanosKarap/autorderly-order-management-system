using System.ComponentModel.DataAnnotations;

namespace FirstProjectAPI.DTOs.Register
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Το Email είναι υποχρεωτικό.")]
        [EmailAddress(ErrorMessage = "Η μορφή του Email δεν είναι έγκυρη.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ο κωδικός είναι υποχρεωτικός.")]
        [MinLength(6, ErrorMessage = "Ο κωδικός πρέπει να είναι τουλάχιστον 6 χαρακτήρες.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Το όνομα του μαγαζιού είναι υποχρεωτικό.")]
        [StringLength(100, ErrorMessage = "Το όνομα είναι πολύ μεγάλο.")]
        public string StoreName { get; set; } = string.Empty;
    }
}