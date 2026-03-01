using FirstProjectAPI.Data;
using FirstProjectAPI.DTOs;
using FirstProjectAPI.Interfaces;
using FirstProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstProjectAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        // 1. Προβολή όλων των κατηγοριών του συγκεκριμένου χρήστη
        public async Task<IEnumerable<CategoryResponseDto>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryResponseDto { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }

        // 2. Δημιουργία κατηγορίας
        public async Task<CategoryResponseDto> CreateAsync(CategoryCreateDto dto, string userId)
        {
            var category = new Category { Name = dto.Name, UserId = userId };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryResponseDto { Id = category.Id, Name = category.Name };
        }

        // 3. Επεξεργασία ονόματος
        public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null) return false;

            category.Name = dto.NewName;
            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Διαγραφή κατηγορίας
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        // Βοηθητική μέθοδος για έλεγχο διπλοεγγραφών
        public async Task<bool> ExistsAsync(string userId, string name, int? excludeId = null)
        {
            return await _context.Categories
                .AnyAsync(c => c.UserId == userId && c.Name == name && c.Id != excludeId);
        }
    }
}