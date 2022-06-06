using AutoMapper;
using FluentValidation;
using WebApi.Application.Contracts.Persistence;

namespace WebApi.Application.DTOs.Category.Validators
{

    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator(ICategoryRepository categoryRepository)
        {
            Include(new CategoryDtoValidator(categoryRepository));

            RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");
        }
    }
}