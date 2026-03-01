using System.ComponentModel.DataAnnotations;

namespace FirstProjectAPI.DTOs
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Το νέο όνομα είναι υποχρεωτικό.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Το όνομα πρέπει να είναι από 3 έως 50 χαρακτήρες.")]
        public string NewName { get; set; } = string.Empty;
    }
}