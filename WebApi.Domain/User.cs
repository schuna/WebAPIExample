using Microsoft.AspNetCore.Identity;

namespace WebApi.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}