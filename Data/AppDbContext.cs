using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Productx> Products { get; set; }
        public DbSet<ProductOptionx> ProductOptions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=App_Data/products.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<Productx>()
                .HasMany(product => product.ProductOptions)
                .WithOne(prductOption => prductOption.Product)
                .IsRequired();
        }
    }
}
