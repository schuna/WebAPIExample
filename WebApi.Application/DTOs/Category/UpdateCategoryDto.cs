using WebApi.Application.DTOs.Common;
using WebApi.Application.DTOs.Product;

namespace WebApi.Application.DTOs.Category
{

    public class UpdateCategoryDto : BaseDto, ICategoryDto
    {
        public string? Name { get; set; }
    }
}