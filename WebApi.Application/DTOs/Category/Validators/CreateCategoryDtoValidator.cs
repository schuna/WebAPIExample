using FluentValidation;
using WebApi.Application.Contracts.Persistence;

namespace WebApi.Application.DTOs.Category.Validators
{

    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator(ICategoryRepository categoryRepository)
        {
            Include(new CategoryDtoValidator(categoryRepository));
        }
    }
}