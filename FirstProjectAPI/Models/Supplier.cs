using FirstProjectAPI.Models;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    // Soft Delete Flag
    public bool IsDeleted { get; set; } = false;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
}