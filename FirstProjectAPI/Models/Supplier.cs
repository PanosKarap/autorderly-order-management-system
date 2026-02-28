namespace FirstProjectAPI.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        // SaaS: Σε ποιον χρήστη ανήκει αυτός ο προμηθευτής;
        public string UserId { get; set; }

        public List<Product> Products { get; set; } = new();
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; } // Το ? σημαίνει προαιρετικό
        public string? PhoneNumber { get; set; }

    }
}
