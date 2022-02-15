using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain.Models;

namespace RefactorThis.Domain
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
    }
}
