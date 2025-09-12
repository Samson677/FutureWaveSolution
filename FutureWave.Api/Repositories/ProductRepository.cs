using FutureWave.Api.Data;
using FutureWave.Api.Entities;
using FutureWave.Api.Repositories.Contracts;
using FutureWave.Models.Dtos;
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

        public async Task<Product> EditProduct(ProductDto updatedProduct)
        {
            var product = await futureWaveDbContext.Products.FindAsync(updatedProduct.Id);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // Update product properties
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Qty = updatedProduct.Qty;
            product.CategoryName = updatedProduct.CategoryName;
            product.Description = updatedProduct.Description;


            // Save changes to the database
            await futureWaveDbContext.SaveChangesAsync();

            return product;
        }



        public async Task<Product> GetProductById(int Id)
        {
            var product = await this.futureWaveDbContext.Products.FindAsync(Id);

            if (product == null)
            {
                // Handle the case where the product is not found
                throw new KeyNotFoundException($"Product with ID {Id} not found.");
            }

            return product;
        }


        public async Task<ProductCartegory> GetProductCartegoryById(int CartegoryId)
        {
            var productCartegory = await this.futureWaveDbContext.ProductCartegories.FindAsync(CartegoryId);

            return productCartegory;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await futureWaveDbContext.Products
                                            .AsNoTracking()
                                            .ToListAsync();
        }

        public async Task<IEnumerable<ProductCartegory>> GetProductsCartegories()
        {
            var Cartegories = await this.futureWaveDbContext.ProductCartegories.ToListAsync();

            return Cartegories;
        }
    }
}
