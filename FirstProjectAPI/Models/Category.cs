namespace FirstProjectAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        // SaaS: Σε ποιον μαγαζάτορα ανήκει αυτή η κατηγορία;
        public string UserId { get; set; }

        // Το όνομα της κατηγορίας (π.χ. "Λαχανικά", "Κάβα", "Αναλώσιμα")
        public string Name { get; set; }

        // Navigation Property: Μια κατηγορία περιέχει πολλά προϊόντα
        public List<Product> Products { get; set; } = new();
    }
}