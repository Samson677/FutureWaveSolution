using Microsoft.AspNetCore.Identity;

namespace FutureWave.Api.Entities.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
