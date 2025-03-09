using FutureWave.Models.Dtos;
using FutureWave.Web.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;


namespace FutureWave.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;
        private readonly AuthService _authService;

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


        public Task<ProductDto> GetProductById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                var token = await _authService.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Authentication token is missing or invalid.");
                }

                Console.WriteLine($"Token: {token}"); // Debugging
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync("api/product");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error fetching products. Status Code: {response.StatusCode}, Response: {errorMessage}");
                    throw new Exception($"Failed to fetch products. Status Code: {response.StatusCode}, Response: {errorMessage}");
                }

                var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                if (products == null)
                {
                    throw new Exception("Failed to deserialize the product data.");
                }

                return products ?? new List<ProductDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
                throw new Exception($"Error fetching products: {ex.Message}");
            }
        }






    }



}
