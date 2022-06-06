using AutoMapper;
using MediatR;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.DTOs.Category.Validators;
using WebApi.Application.Exceptions;
using WebApi.Application.Features.Categories.Requests.Commands;

namespace WebApi.Application.Features.Categories.Handlers.Commands;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateCategoryDtoValidator(_categoryRepository);
        var validationResult = await validator.ValidateAsync(command.CategoryDto, cancellationToken);

        if (validationResult is { IsValid: false })
            throw new ValidationException(validationResult);

        var category = await _categoryRepository.Get(command.Id);
        _mapper.Map(command.CategoryDto, category);
        await _categoryRepository.Update(category);
        return Unit.Value;
    }
}