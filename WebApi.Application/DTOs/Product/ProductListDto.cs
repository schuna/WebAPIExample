using WebApi.Application.DTOs.Category;
using WebApi.Application.DTOs.Common;

namespace WebApi.Application.DTOs.Product
{

    public class ProductListDto : BaseDto
    {
        public string? Sku { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryDto? Category { get; set; }
    }
}