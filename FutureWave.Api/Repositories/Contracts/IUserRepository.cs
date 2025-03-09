using Microsoft.AspNetCore.Identity;

namespace FutureWave.Api.Repositories.Contracts
{
    public interface IUserRepository
    {
     Task<IdentityUser> FindByEmailAsync(string email);
     Task<SignInResult> LoginAsync(IdentityUser user, string password);

    }
}
