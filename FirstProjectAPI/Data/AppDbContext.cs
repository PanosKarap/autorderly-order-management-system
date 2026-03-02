using FirstProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FirstProjectAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Supplier>().HasQueryFilter(s => !s.IsDeleted);

            // 1. Ρύθμιση του Composite Key για τον ProductSupplier
            // Λέμε στην EF ότι το "κλειδί" είναι ο συνδυασμός ProductId και SupplierId
            modelBuilder.Entity<ProductSupplier>()
                .HasKey(ps => new { ps.ProductId, ps.SupplierId });

            // 2. Ρύθμιση Precision για το Price (Decimal 18,2)
            // Πολύ σημαντικό για να μην χάνουμε δεκαδικά και να μην έχουμε warnings
            modelBuilder.Entity<ProductSupplier>()
                .Property(ps => ps.Price)
                .HasPrecision(18, 2);

            // 3. Unique Index για Categories (το είχες ήδη σωστά)
            modelBuilder.Entity<Category>()
                .HasIndex(c => new { c.UserId, c.Name })
                .IsUnique();

            // 4. Προαιρετικό: Unique Index για Products ανά Χρήστη
            // Δεν θέλουμε ο ίδιος χρήστης να έχει δύο προϊόντα με το ίδιο όνομα
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.UserId, p.Name })
                .IsUnique();
        }
    }
}