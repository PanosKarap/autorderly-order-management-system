namespace FirstProjectAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Unit { get; set; } = "Pieces";
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Ο Default Προμηθευτής για να ξέρει το "Orderly" πού να στείλει το email αυτόματα
        public int DefaultSupplierId { get; set; }

        // Η λίστα με όλους τους διαθέσιμους προμηθευτές για αυτό το προϊόν
        // και τις τιμές τους
        public List<ProductSupplier> ProductSuppliers { get; set; } = new();
    }
}