using FutureWave.Models.Dtos;
using FutureWave.Web.Services.Contracts;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace FutureWave.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService _localStorage;

        public LoginDto loginDto = new LoginDto();

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            _localStorage = localStorage;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/authentication/login", loginDto);
                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                    if (loginResponse != null)
                    {

                        await _localStorage.SetItemAsync("authToken", loginResponse.Token);
                        await _localStorage.SetItemAsync("email", loginResponse.Email);
                        await _localStorage.SetItemAsync("role", loginResponse.Roles);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);

                        return loginResponse;
                    }
                }
            }
            catch (Exception)
            {

            }


            return null;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var response = await httpClient.PostAsJsonAsync("api/Authentication/Register", registerDto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }



}

