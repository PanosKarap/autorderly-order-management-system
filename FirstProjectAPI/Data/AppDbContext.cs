using Microsoft.EntityFrameworkCore;
using FirstProjectAPI.Models; // <-- Για να βλέπει το Product σου

namespace FirstProjectAPI.Data
{
    // Αυτή η κλάση ΕΙΝΑΙ η βάση δεδομένων σου
    public class AppDbContext : DbContext
    {
        // Ο κατασκευαστής (Constructor) - χρειάζεται για να περάσουμε ρυθμίσεις αργότερα
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Εδώ λέμε: "Θέλω έναν Πίνακα (DbSet) που θα έχει μέσα Products"
        public DbSet<Product> Products { get; set; }
    }
}