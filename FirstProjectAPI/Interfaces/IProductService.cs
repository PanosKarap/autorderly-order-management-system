using FirstProjectAPI.Models;
using FirstProjectAPI.DTOs; // Θα φτιάξουμε τα DTOs αμέσως μετά

namespace FirstProjectAPI.Interfaces
{
    public interface IProductService
    {
        // 1. Λήψη όλων των προϊόντων του χρήστη (με φιλτράρισμα ανά κατηγορία αν θέλουμε)
        //Task<IEnumerable<ProductResponseDto>> GetUserProductsAsync(string userId, int? categoryId = null);

        //// 2. Λήψη ενός συγκεκριμένου προϊόντος με όλους τους προμηθευτές του
        //Task<ProductResponseDto?> GetProductByIdAsync(int productId, string userId);

        //// 3. Δημιουργία νέου προϊόντος
        //Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto, string userId);

        //// 4. Ενημέρωση βασικών στοιχείων προϊόντος (Όνομα, Unit, κτλ)
        //Task<ProductResponseDto?> UpdateProductAsync(int productId, ProductUpdateDto dto, string userId);

        // 5. Διαγραφή προϊόντος
        Task<bool> DeleteProductAsync(int productId, string userId);

        // --- Εξειδικευμένες μέθοδοι για την αγορά (Suppliers & Prices) ---

        // 6. Προσθήκη ή ενημέρωση τιμής από συγκεκριμένο προμηθευτή
        Task<bool> UpdateProductPriceForSupplierAsync(int productId, int supplierId, decimal newPrice, string userId);

        // 7. Αλλαγή του Default Προμηθευτή
        Task<bool> SetDefaultSupplierAsync(int productId, int supplierId, string userId);
    }
}