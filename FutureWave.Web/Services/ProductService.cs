using FutureWave.Models.Dtos;
using FutureWave.Web.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Security.Cryptography.X509Certificates;
using FutureWave.Models.Dtos.FutureWave.Models.Dtos;


namespace FutureWave.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;
        private readonly AuthService _authService;

        public ProductDto? SelectedProduct { get; set; }

        public ProductService(HttpClient httpClient, AuthService authService)
        {
            this.httpClient = httpClient;
            _authService = authService;
        }
        public async Task AddProductAsync(ProductDto productDto)
        {
            var token = await _authService.GetTokenAsync();

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.PostAsJsonAsync("Api/Product/AddProduct", productDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to add product. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding product: {ex.Message}");
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"Api/Product/Delete/{id}");

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }



        public async Task<ProductDto> EditProduct(ProductDto productDto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync("Api/Product/Edit", productDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProductDto>(); // Expect a single product, not a list
                }
                else
                {
                    throw new Exception($"Failed to edit product. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while editing the product: {ex.Message}");
            }
        }


        public async Task<ProductDto> GetProductById(int Id)
        {
            try
            {
                // Removed token handling and Authorization header setting
                var response = await httpClient.GetAsync($"api/product/{Id}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to fetch product. Status Code: {response.StatusCode}, Response: {errorMessage}");
                }

                var product = await response.Content.ReadFromJsonAsync<ProductDto>();

                return product ?? new ProductDto(); // Ensure it never returns null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching product by ID: {ex.Message}");
                throw new Exception($"Error fetching product by ID: {ex.Message}");
            }
        }


        public async Task<Result<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var token = await _authService.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    return Result<IEnumerable<ProductDto>>.Fail("Authentication token is missing or invalid.");
                }

                Console.WriteLine($"Token: {token}"); // Debugging
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync("api/product");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error fetching products. Status Code: {response.StatusCode}, Response: {errorMessage}");
                    return Result<IEnumerable<ProductDto>>.Fail($"Failed to fetch products. Status Code: {response.StatusCode}, Response: {errorMessage}");
                }

                var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                if (products == null)
                {
                    return Result<IEnumerable<ProductDto>>.Fail("Failed to deserialize the product data.");
                }

                return Result<IEnumerable<ProductDto>>.Success(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
                return Result<IEnumerable<ProductDto>>.Fail($"Error fetching products: {ex.Message}");
            }
        }

        public string GetImageSrc(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return "/Images/no-image.png";
            }
            return $"data:image/jpeg;base64,{Convert.ToBase64String(imageData)}";
        }



    }
}
