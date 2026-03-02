using FirstProjectAPI.DTOs.Supplier;

namespace FirstProjectAPI.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierResponseDto>> GetAllAsync(string userId);
        Task<SupplierResponseDto> CreateAsync(SupplierRequestDto dto, string userId);
        Task<SupplierResponseDto> UpdateAsync(int id, SupplierRequestDto dto, string userId);
        Task DeleteAsync(int id, string userId);
        Task<bool> ExistsAsync(string userId, string name, int? excludeId = null);
    }
}