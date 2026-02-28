namespace FirstProjectAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        // Κατάσταση: Pending, Completed, Cancelled
        public string Status { get; set; } = "Pending";

        // Η λίστα με τα πράγματα που παρήγγειλα
        public List<OrderItem> Items { get; set; } = new();
    }
}
