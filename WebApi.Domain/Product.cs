using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Domain.Common;

namespace WebApi.Domain
{

    public class Product : BaseDomainEntity
    {
        public string? Sku { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}