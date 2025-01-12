namespace Product.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Prod = Product.Domain.Entities;

    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Prod.Product> Products { get; set; }
    }
}
