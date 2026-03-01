using FirstProjectAPI.DTOs;
using FirstProjectAPI.DTOs.Register;
using FirstProjectAPI.Services;
using FirstProjectAPI.Services.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public UsersController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("RegisterClient")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _registerService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var response = new RegisterResponseDto
            {
                Email = dto.Email,
                StoreName = dto.StoreName
            };

            return Ok(response);
        }
    }
}