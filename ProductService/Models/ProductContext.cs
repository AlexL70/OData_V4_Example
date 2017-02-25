using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ProductService.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base($"name={nameof(ProductContext)}") { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
