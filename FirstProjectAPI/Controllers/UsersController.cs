using FirstProjectAPI.DTOs.Register;
using FirstProjectAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = $"Το μαγαζί '{dto.StoreName}' δημιουργήθηκε με επιτυχία!" });
        }

        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var stores = await _userService.GetAllStoresAsync();
            return Ok(stores);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update([FromBody] RegisterResponseDto dto)
        {
            var result = await _userService.UpdateStoreAsync(dto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Τα στοιχεία του μαγαζιού ενημερώθηκαν επιτυχώς!", Data = dto });
        }

        [HttpDelete("DeleteUser/{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            var result = await _userService.DeleteStoreAsync(email);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = $"Το μαγαζί με email {email} διαγράφηκε." });
        }
    }
}