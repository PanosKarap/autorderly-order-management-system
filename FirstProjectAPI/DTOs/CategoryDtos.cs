using System.ComponentModel.DataAnnotations;

namespace FirstProjectAPI.DTOs
{
    // Αυτό που χρησιμοποιούμε για το POST
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Το όνομα είναι υποχρεωτικό")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }
    }

    // --- ΠΡΟΣΘΕΣΕ ΑΥΤΟ ΤΩΡΑ ---
    // Αυτό που χρησιμοποιούμε για το GET
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Παρατήρησε ότι ΔΕΝ βάζουμε UserId ή τη λίστα Products εδώ
    }

    public class CategoryUpdateDto
    {
        public string UserId { get; set; }
        public string NewName { get; set; }
    }
}