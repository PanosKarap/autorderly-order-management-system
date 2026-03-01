using FirstProjectAPI.DTOs;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllByUserIdAsync(string userId);
    Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto, string userId);
    Task<bool> UpdateAsync(int id, CategoryUpdateDto dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);
    Task<bool> ExistsAsync(string userId, string name, int? excludeId = null);
}