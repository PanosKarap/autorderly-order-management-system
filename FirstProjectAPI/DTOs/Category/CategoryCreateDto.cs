using System.ComponentModel.DataAnnotations;

namespace FirstProjectAPI.DTOs
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Το όνομα της κατηγορίας είναι υποχρεωτικό.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Το όνομα πρέπει να είναι από 3 έως 50 χαρακτήρες.")]
        public string Name { get; set; } = string.Empty;
    }
}