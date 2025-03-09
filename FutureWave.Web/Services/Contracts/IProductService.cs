using FutureWave.Models.Dtos;

namespace FutureWave.Web.Services.Contracts
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();
        public Task<ProductDto> GetProductById(int Id);
        public Task AddProductAsync(ProductDto productDto);
    }
}
