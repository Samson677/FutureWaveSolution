using Microsoft.AspNetCore.Identity;

namespace FutureWave.Api.Entities.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
