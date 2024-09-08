using BestSellerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BestSellerApp.Services
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
        public DbSet<Product> products { get; set; }
    }
}
