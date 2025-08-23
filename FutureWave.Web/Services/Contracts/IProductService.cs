using FutureWave.Models.Dtos;
using FutureWave.Models.Dtos.FutureWave.Models.Dtos;

namespace FutureWave.Web.Services.Contracts
{
    public interface IProductService
    {
        public Task<Result<IEnumerable<ProductDto>>> GetProducts();
        public Task<ProductDto> GetProductById(int Id);
        public Task AddProductAsync(ProductDto productDto);
        public Task<ProductDto> EditProduct(ProductDto productDto);
        public  Task<bool> DeleteProduct(int id);
        public string GetImageSrc(byte[] imageData);
        public ProductDto? SelectedProduct { get; set; }
    }
}
