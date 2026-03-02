using System.ComponentModel.DataAnnotations;

namespace FirstProjectAPI.DTOs.Supplier
{
    // Αυτό που στέλνει η React για Create/Update
    public class SupplierRequestDto
    {
        [Required(ErrorMessage = "Το όνομα του προμηθευτή είναι υποχρεωτικό")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        [EmailAddress(ErrorMessage = "Το Email δεν είναι έγκυρο")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Ο αριθμός τηλεφώνου δεν είναι έγκυρος")]
        public string? PhoneNumber { get; set; }
    }
}
