using FluentValidation;
using WebApi.Application.Contracts.Persistence;

namespace WebApi.Application.DTOs.Product.Validators
{

    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator(IProductRepository productRepository)
        {
            Include(new ProductDtoValidator(productRepository));
        }
    }
}