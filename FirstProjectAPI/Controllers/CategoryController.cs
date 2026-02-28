using FirstProjectAPI.Data;
using FirstProjectAPI.DTOs;
using FirstProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
        {
            var exists = await _context.Categories
                .AnyAsync(c => c.UserId == dto.UserId && c.Name == dto.Name);
            if (exists) {
                return BadRequest(new { Message = "Υπάρχει ήδη κατηγορία με αυτό το όνομα για αυτόν τον χρήστη." });
            }

            var category = new Category
            {
                Name = dto.Name,
                UserId = dto.UserId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Η κατηγορία δημιουργήθηκε!", Id = category.Id });
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetAllCategories(string userId)
        {
            var categories = await _context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var response = categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == id);

            if (category == null)
                return NotFound(new { Message = "Η κατηγορία δεν βρέθηκε για αυτόν τον χρήστη." });

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Η κατηγορία διαγράφηκε επιτυχώς." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto dto)
        {
            var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == id && c.UserId == dto.UserId);
            if (category == null)
                return NotFound(new { Message = "Η κατηγορία δεν βρέθηκε για αυτόν τον χρήστη." });

            var exists = await _context.Categories
                .AnyAsync(c => c.UserId == dto.UserId && c.Name == dto.NewName && c.Id != id);
            if (exists)
                return BadRequest(new { Message = "Υπάρχει ήδη άλλη κατηγορία με αυτό το όνομα." });

            category.Name = dto.NewName;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Η κατηγορία ενημερώθηκε επιτυχώς.", NewName = category.Name });
        }
    }
}