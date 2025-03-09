using FutureWave.Api.Data;
using FutureWave.Api.Entities;
using FutureWave.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FutureWave.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly FutureWaveDbContext futureWaveDbContext;

        public ProductRepository(FutureWaveDbContext futureWaveDbContext)
        {
            this.futureWaveDbContext = futureWaveDbContext;
        }

        public async Task AddProduct(Product product)
        {
       
            await futureWaveDbContext.Products.AddAsync(product);

     
            await futureWaveDbContext.SaveChangesAsync();
        }


        public async Task<bool> DeleteProduct(int Id)
        {
            var product = await futureWaveDbContext.Products.FindAsync(Id);

            futureWaveDbContext.Products.Remove(product);
            await futureWaveDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Product> EditProduct(int Id)
        {
            var product = await futureWaveDbContext.Products.FindAsync(Id);
            return product;
        }

        public async Task<Product> GetProductById(int Id)
        {
            var product = await this.futureWaveDbContext.Products.FindAsync(Id);

            return product;
        }

        public async Task<ProductCartegory> GetProductCartegoryById(int CartegoryId)
        {
            var productCartegory = await this.futureWaveDbContext.ProductCartegories.FindAsync(CartegoryId);

            return productCartegory;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
           var products = await this.futureWaveDbContext.Products.ToListAsync();

            return products;
        }

        public async Task<IEnumerable<ProductCartegory>> GetProductsCartegories()
        {
           var Cartegories = await this.futureWaveDbContext.ProductCartegories.ToListAsync();

            return Cartegories;
        }
    }
}
