using System.Data.Entity;

namespace ProductService.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base($"name={nameof(ProductContext)}") { }

        public DbSet<Product> Products { get; set; }
    }
}
