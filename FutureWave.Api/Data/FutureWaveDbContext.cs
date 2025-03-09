using FutureWave.Api.Entities;
using Microsoft.EntityFrameworkCore;


namespace FutureWave.Api.Data
{
    public class FutureWaveDbContext:DbContext
    {
        public FutureWaveDbContext(DbContextOptions<FutureWaveDbContext> options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ProductCartegory> ProductCartegories { get; set; }
    }
}
