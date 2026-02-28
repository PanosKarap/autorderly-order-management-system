namespace FirstProjectAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string UserId { get; set; } // SaaS: Ποιανού είναι το προϊόν;
        public List<Supplier> ProvidingSuppliers { get; set; } = new(); // Σχέση: Από ποιους προμηθευτές το παίρνω;

        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } // Αποθηκεύουμε το URL/Path της εικόνας, όχι την ίδια την εικόνα στη βάση
        public string Unit { get; set; } // π.χ. "Kg", "Lt", "Pieces"
    }
}