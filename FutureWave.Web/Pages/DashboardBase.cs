using FutureWave.Models.Dtos;
using FutureWave.Web.Services;
using FutureWave.Web.Services.Contracts;
using FutureWave.Web.SharedServices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FutureWave.Web.Pages
{
    public class DashboardBase : ComponentBase
    {
        [Inject]
        public IProductService? ProductService { get; set; }
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;
        [Inject]
        public SharedState? SharedState { get; set; }
        public IEnumerable<ProductDto>? products;
        public bool isSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        private decimal _selectedPriceRange = 0;
        private string? _selectedProduct;
        public IEnumerable<ProductDto>? FilteredProduct => products?.Where(p =>
        (string.IsNullOrEmpty(_selectedProduct) || p.Name == _selectedProduct) &&
        (_selectedPriceRange == 0 || p.Price <= _selectedPriceRange));













        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (ProductService is not null)
                {
                    var result = await ProductService.GetProducts();

                    if (result.IsSuccess && result.Data is not null)
                    {
                        products = result.Data;
                        SharedState.ProductState = result.Data;




                    }
                    else
                    {
                        ErrorMessage = result.ErrorMessage ?? "Failed to fetch products.";
                    }
                }
                else
                {
                    ErrorMessage = "Product service is not available.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error fetching products: {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                if (ProductService is not null)
                {
                    await ProductService.DeleteProduct(id);


                    var fetchResult = await ProductService.GetProducts();

                    if (fetchResult.IsSuccess && fetchResult.Data is not null)
                    {
                        products = fetchResult.Data;
                        isSuccess = true;
                        ErrorMessage = "Product deleted successfully.";
                    }
                    else
                    {
                        ErrorMessage = fetchResult.ErrorMessage ?? "Failed to fetch updated products.";
                        isSuccess = false;
                    }
                }
                else
                {
                    ErrorMessage = "Product service is not available.";
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: Failed to delete product. {ex.Message}";
                isSuccess = false;
            }

            StateHasChanged();
        }


        public async Task EditAsync(ProductDto productDto)
        {
            try
            {



                Navigation.NavigateTo($"/edit/{productDto.Id}");


            }
            catch (Exception ex)
            {
                ErrorMessage = $"Operation failed. {ex.Message}";
            }
        }


        protected void SelectedRange(decimal range)
        {
            _selectedPriceRange = range;
        }
        protected void SelectedItem(string name)
        {
            _selectedProduct = name;


        }



    }
}
