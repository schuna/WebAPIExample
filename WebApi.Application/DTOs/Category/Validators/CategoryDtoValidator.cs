using FluentValidation;
using WebApi.Application.Contracts.Persistence;

namespace WebApi.Application.DTOs.Category.Validators
{

    public class CategoryDtoValidator : AbstractValidator<ICategoryDto>
    {
        private ICategoryRepository CategoryRepository { get; }

        public CategoryDtoValidator(ICategoryRepository categoryRepository)
        {
            CategoryRepository = categoryRepository;
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}