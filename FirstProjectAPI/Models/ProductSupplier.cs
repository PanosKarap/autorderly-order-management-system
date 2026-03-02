using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProjectAPI.Models
{
    // Ο "μαγικός" πίνακας που λύνει το πρόβλημα της τιμής ανά προμηθευτή
    public class ProductSupplier
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        // Εδώ αποθηκεύουμε την τιμή που δίνει ο συγκεκριμένος προμηθευτής
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
