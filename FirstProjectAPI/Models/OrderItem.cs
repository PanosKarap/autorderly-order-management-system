namespace FirstProjectAPI.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Quantity { get; set; } // Πόσα πήρα;
        public decimal UnitPriceSnapshot { get; set; } // Πόσο έκανε ΤΟΤΕ;
    }
}
