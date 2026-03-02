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

        // Βοηθητική μέθοδος για έλεγχο διπλοεγγραφών
        public async Task<bool> ExistsAsync(string userId, string name, int? excludeId = null)
        {
            return await _context.Categories
                .AnyAsync(c => c.UserId == userId && c.Name == name && c.Id != excludeId);
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
            if (await ExistsAsync(userId, dto.Name))
            {
                throw new InvalidOperationException("Υπάρχει ήδη κατηγορία με αυτό το όνομα.");
            }
            var category = new Category
            {
                Name = dto.Name,
                UserId = userId
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new CategoryResponseDto { Id = category.Id, Name = category.Name };
        }

        // 3. Επεξεργασία ονόματος κατηγορίας
        public async Task<CategoryResponseDto> UpdateAsync(int id, CategoryUpdateDto dto, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (category == null)
            {
                throw new KeyNotFoundException("Η κατηγορία δεν βρέθηκε.");
            }

            if (await ExistsAsync(userId, dto.NewName, id))
            {
                throw new InvalidOperationException("Υπάρχει ήδη άλλη κατηγορία με αυτό το όνομα.");
            }

            category.Name = dto.NewName;
            await _context.SaveChangesAsync();
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        // 4. Διαγραφή κατηγορίας
        public async Task DeleteAsync(int id, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (category == null) throw new KeyNotFoundException("Η κατηγορία δεν βρέθηκε.");

            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
            {
                throw new InvalidOperationException("Δεν μπορείτε να διαγράψετε την κατηγορία γιατί περιέχει προϊόντα.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}