using FutureWave.Models.Dtos;
using FutureWave.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace FutureWave.Web.Pages.Auth
{


    public class LoginBase : ComponentBase
    {
        [Inject]
        IAuthService AuthService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        public LoginDto loginDto = new LoginDto();
        public bool isLoading = false;
        public bool loginError = false;


        public async Task<LoginResponseDto> LoginAsync()
        {
            isLoading = true;
            loginError = false;

            // Call the AuthService to perform login and retrieve the response (which contains token and user details)
            var response = await AuthService.LoginAsync(loginDto);

            isLoading = false;

            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                loginError = true;
                return null; // Login failed, returning null as response
            }
            else
            {

                NavigationManager.NavigateTo("/Product");

                return response; // Return the LoginResponseDto containing token and user details
            }
        }




    }
}
