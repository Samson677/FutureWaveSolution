using FutureWave.Api.Entities;
using FutureWave.Models.Dtos;

namespace FutureWave.Api.Repositories.Contracts
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetProducts();
        public Task<Product> GetProductById(int Id);
        public Task<IEnumerable<ProductCartegory>> GetProductsCartegories();
        public Task<ProductCartegory>  GetProductCartegoryById(int CartegoryId);
        public Task<bool> DeleteProduct(int Id);
        public Task<Product> EditProduct(ProductDto productDto);
        public Task AddProduct(Product product);
    }
}
