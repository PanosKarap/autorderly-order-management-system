using FirstProjectAPI.Models; // Βάλε το σωστό namespace των Models σου
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FirstProjectAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // Ο constructor που παίρνει τις ρυθμίσεις (π.χ. το Connection String) από το Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Εδώ λέμε στο Entity Framework ποιους πίνακες θέλουμε να φτιάξει.
        // ΠΡΟΣΟΧΗ: Τα ονόματα των DbSet είναι στον πληθυντικό (όπως θα λέγονται στη βάση).
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Αυτή η γραμμή λέει στη βάση: 
            // "Μην επιτρέψεις το ίδιο Name για το ίδιο UserId"
            modelBuilder.Entity<Category>()
                .HasIndex(c => new { c.UserId, c.Name })
                .IsUnique();
        }
    }
}