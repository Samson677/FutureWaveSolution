using FutureWave.Models.Dtos;
using FutureWave.Web.Services;
using FutureWave.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;

namespace FutureWave.Web.Pages.Product
{
     
    

    
    public class EditProductBase : ComponentBase 
    {
      

        public EditProductBase()
        {
            
        }
        [Inject]
        public IProductService ProductService { get; set; }
        public ProductDto ProductToEdit  = new();
        public bool isSucess = false;
        public bool productError = false;
        [Parameter] public int Id { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            ProductToEdit = await ProductService.GetProductById(Id);
        }





        public async Task SaveAsync()
        {
            try
            {
                await ProductService.EditProduct(ProductToEdit);

                isSucess = true;

                // Navigate only if successful
                if (isSucess)
                {
                    Navigation.NavigateTo("/");
                    Console.WriteLine($"Executed:");
                }
            }
            catch (Exception ex)
            {
                productError = true;

                // Optional: Log the error or display a message
                Console.WriteLine($"Error while saving product: {ex.Message}");
            }
        }

    }
}
