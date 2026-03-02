using FirstProjectAPI.DTOs;
using FirstProjectAPI.Interfaces;
using FirstProjectAPI.Services;
using Humanizer;
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            try
            {
                var userId = GetUserId();
                var result = await _categoryService.CreateAsync(dto, userId);
                return Ok(new
                {
                    Message = "Η κατηγορία δημιουργήθηκε επιτυχώς!",
                    Data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // --- PUT: api/Categories/Update/5 ---
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            try
            {
                var userId = GetUserId();
                var result = await _categoryService.UpdateAsync(id, dto, userId);
                return Ok(new
                {
                    Message = "To όνομα της κατηγορίας ενημερώθηκε!",
                    Data = result
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // --- DELETE: api/Categories/Delete/5 ---
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetUserId();
                await _categoryService.DeleteAsync(id, userId);
                return Ok(new { Message = "Η κατηγορία διαγράφηκε οριστικά." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}