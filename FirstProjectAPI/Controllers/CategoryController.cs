using FirstProjectAPI.DTOs;
using FirstProjectAPI.Interfaces;
using FirstProjectAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "StoreOwner")] // Κλειδωμένο για μαγαζιά
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Helper μέθοδος για να παίρνουμε το UserId από το Token
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // --- GET: api/Categories/All ---
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllByUserIdAsync(GetUserId());
            return Ok(categories);
        }

        // --- POST: api/Categories/Create ---
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            var userId = GetUserId();

            if (await _categoryService.ExistsAsync(userId, dto.Name))
                return BadRequest(new { Message = "Υπάρχει ήδη κατηγορία με αυτό το όνομα." });

            await _categoryService.CreateAsync(dto, userId);
            return Ok(new { Message = "Η κατηγορία δημιουργήθηκε επιτυχώς!"});
        }

        // --- PUT: api/Categories/Update/5 ---
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            var userId = GetUserId();

            if (await _categoryService.ExistsAsync(userId, dto.NewName, id))
                return BadRequest(new { Message = "Το όνομα χρησιμοποιείται ήδη σε άλλη κατηγορία." });

            var success = await _categoryService.UpdateAsync(id, dto, userId);
            if (!success) return NotFound(new { Message = "Η κατηγορία δεν βρέθηκε ή δεν έχετε δικαίωμα πρόσβασης." });

            return Ok(new { Message = "Το όνομα της κατηγορίας ενημερώθηκε." });
        }

        // --- DELETE: api/Categories/Delete/5 ---
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteAsync(id, GetUserId());
            if (!success) return NotFound(new { Message = "Η κατηγορία δεν βρέθηκε ή δεν έχετε δικαίωμα πρόσβασης." });

            return Ok(new { Message = "Η κατηγορία διαγράφηκε οριστικά." });
        }
    }
}