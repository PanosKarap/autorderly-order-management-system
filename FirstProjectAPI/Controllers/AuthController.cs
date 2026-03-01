using FirstProjectAPI.DTOs.Login;
using FirstProjectAPI.Services.Login;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result == null)
        {
            return Unauthorized(new { Message = "Λάθος Email ή Κωδικός." });
        }

        // Επιστροφή του token και του ονόματος του καταστήματος μαζί
        // με την ημερομηνία λήξης του token και τον κωδικό επιτυχίας
        return Ok(result);
    }
}