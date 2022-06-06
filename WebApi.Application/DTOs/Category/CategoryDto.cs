using WebApi.Application.DTOs.Common;
using WebApi.Application.DTOs.Product;

namespace WebApi.Application.DTOs.Category
{

    public class CategoryDto : BaseDto
    {
        public string? Name { get; set; }
        public virtual List<ProductDto>? Products { get; set; }
    }
}