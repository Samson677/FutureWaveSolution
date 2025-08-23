using FutureWave.Models.Dtos;
using FutureWave.Web.Services;
using FutureWave.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace FutureWave.Web.Pages.Auth
{
    public class RegisterBase: ComponentBase
    {

        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject ] IAuthService authService { get; set; }

        public bool isLoading = false;
        public bool registerError = false;
        public RegisterDto registerDto = new RegisterDto();

        public async Task RegisterAsync()
        {
            isLoading = true;
            registerError = false;


            var response = await authService.RegisterAsync(registerDto);

            isLoading = false;

            if (response == null)
            {
                registerError = true; 
            }
            else
            {
                NavigationManager.NavigateTo("/login"); 
            }
        }
    }
}
