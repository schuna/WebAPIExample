using WebApi.Domain.Common;

namespace WebApi.Domain
{

    public class Category : BaseDomainEntity
    {
        public virtual List<Product>? Products { get; set; }
    }
}