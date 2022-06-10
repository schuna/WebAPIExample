using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Domain
{
    public class RefreshToken
    {
        [Key] public string Id { get; set; } = null!;

        public string Token { get; set; } = null!;
        public string JwtId { get; set; } = null!;
        public bool IsRevoked { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateExpire { get; set; }
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}