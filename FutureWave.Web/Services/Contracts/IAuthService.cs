using FutureWave.Models.Dtos;

namespace FutureWave.Web.Services.Contracts
{
    public interface IAuthService
    {
           
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
       public Task<bool> RegisterAsync(RegisterDto registerDto);
        public Task<string> GetTokenAsync();
        public Task LogoutAsync();
    }
}
