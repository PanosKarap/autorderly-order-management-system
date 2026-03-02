using FirstProjectAPI.Interfaces;
using FirstProjectAPI.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProjectAPI.Controllers
{
    [Authorize(Roles = "StoreOwner")]
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _supplierService.GetAllAsync(GetUserId()));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SupplierRequestDto dto)
        {
            try
            {
                var result = await _supplierService.CreateAsync(dto, GetUserId());
                return Ok(new { Message = "Επιτυχής δημιουργία!", Data = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Update(int id, SupplierRequestDto dto)
        {
            try
            {
                var result = await _supplierService.UpdateAsync(id, dto, GetUserId());
                return Ok(new { Message = "Ενημερώθηκε!", Data = result });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { Message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _supplierService.DeleteAsync(id, GetUserId());
                return Ok(new { Message = "Ο προμηθευτής διαγράφτηκε." });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { Message = ex.Message }); }
        }
    }
}