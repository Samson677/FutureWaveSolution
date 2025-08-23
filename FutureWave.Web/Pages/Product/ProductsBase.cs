using FutureWave.Models.Dtos;
using FutureWave.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutureWave.Web.Pages.Product
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService? ProductService { get; set; } 

        public IEnumerable<ProductDto>? Products { get; set; }
        public int ProductCount { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (ProductService != null)
                {
                    var result = await ProductService.GetProducts();

                    if (result.IsSuccess && result.Data != null)
                    {
                        Products = result.Data;
                        ProductCount = Products.Count();
                    }
                    else
                    {
                        Console.WriteLine($"Error fetching products: {result.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error fetching products: {ex.Message}");
            }
        }
    }
}
