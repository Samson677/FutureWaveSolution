using FutureWave.Models.Dtos;
using FutureWave.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace FutureWave.Web.Pages.Product
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService? ProductService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Products = await ProductService.GetProducts();
            }
            catch (Exception)
            {

                throw;
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
