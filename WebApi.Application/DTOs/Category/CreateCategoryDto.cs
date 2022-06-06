using WebApi.Application.DTOs.Product;

namespace WebApi.Application.DTOs.Category
{

    public class CreateCategoryDto : ICategoryDto
    {
        public string? Name { get; set; }
        public virtual List<ProductDto>? Products { get; set; }
    }
}