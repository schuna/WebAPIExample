using Newtonsoft.Json;

namespace WebApi.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        [JsonIgnore] public string PasswordHash { get; set; } = null!;
        public virtual List<Order>? Orders { get; set; }
    }
}