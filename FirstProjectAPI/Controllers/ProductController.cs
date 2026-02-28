using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstProjectAPI.Data;
using FirstProjectAPI.Models;

namespace FirstProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // 1. Εδώ φέρνουμε τη Βάση Δεδομένων (σύνδεση)
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // 2. GET: Φέρε μου ΟΛΑ τα προϊόντα από τη βάση
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // SQL Μετάφραση: SELECT * FROM Products
            return await _context.Products.ToListAsync();
        }

        // 3. POST: Βάλε νέο προϊόν στη βάση
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Προσθέτουμε το προϊόν στην "αναμονή"
            _context.Products.Add(product);

            // SQL Μετάφραση: INSERT INTO Products VALUES (...)
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducts", new { id = product.Id }, product);
        }
    }
}