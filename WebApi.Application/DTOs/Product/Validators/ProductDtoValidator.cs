using FluentValidation;
using WebApi.Application.Contracts.Persistence;

namespace WebApi.Application.DTOs.Product.Validators {
    public class ProductDtoValidator : AbstractValidator<IProductDto>
    {
        public IProductRepository ProductRepository { get; }

        public ProductDtoValidator(IProductRepository productRepository)
        {
            ProductRepository = productRepository;
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyNAme} must be greater than or equal to 0")
                .ScalePrecision(4, 18);

        }
    }
}