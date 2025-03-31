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

            modelBuilder.Entity<Prod.Product>().HasData(
                new Prod.Product
                {
                    Id = 1,
                    Name = "Sample Product 1",
                    Description = "Description for Sample Product 1",
                    Price = 9.99m,
                    CreatedDate = DateTime.UtcNow
                },
                new Prod.Product
                {
                    Id = 2,
                    Name = "Sample Product 2",
                    Description = "Description for Sample Product 2",
                    Price = 19.99m,
                    CreatedDate = DateTime.UtcNow
                }
            );
        }

        public DbSet<Prod.Product> Products { get; set; }
    }
}
