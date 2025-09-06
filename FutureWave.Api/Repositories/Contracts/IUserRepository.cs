using FutureWave.Api.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace FutureWave.Api.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        Task<SignInResult> LoginAsync(User user, string password);

    }
}
