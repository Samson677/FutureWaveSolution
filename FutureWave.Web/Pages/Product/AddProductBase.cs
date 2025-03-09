using FutureWave.Models.Dtos;
using FutureWave.Web.Services;
using FutureWave.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FutureWave.Web.Pages.Product
{
    public class AddProductBase: ComponentBase
    {
        [Inject] 
        IProductService productService {  get; set; }

        public bool isLoading = false;
        public bool isSucess = false;
        public bool productError = false;
        public ProductDto productDto = new();
      

        public async Task AddProductAsync()
        {
            isLoading = true;
            productError = false;
            isSucess = false;

            try
            {
                await productService.AddProductAsync(productDto); 

              
                        isSucess = true;
    }
            catch (Exception)
            {
                productError = true;
            }
            finally
            {
                isLoading = false; 

    }
        }





        public async Task OnFileChange(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null)
            {
                // Convert the file to byte array
                var buffer = new byte[file.Size];
                using (var stream = file.OpenReadStream())
                {
                    await stream.ReadAsync(buffer, 0, (int)file.Size);
                }
                productDto.Image = buffer; 
            }
        }
    }
}
