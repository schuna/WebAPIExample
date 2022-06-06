using AutoMapper;
using MediatR;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.DTOs.Category.Validators;
using WebApi.Application.Features.Categories.Requests.Commands;
using WebApi.Application.Response;
using WebApi.Domain;

namespace WebApi.Application.Features.Categories.Handlers.Commands;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, BaseCommandResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<BaseCommandResponse> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new CreateCategoryDtoValidator(_categoryRepository);
        var validationResult = await validator.ValidateAsync(command.CategoryDto, cancellationToken);
        if (validationResult is { IsValid: false })
        {
            response.Success = false;
            response.Message = "Creating failed";
            response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
        }

        var category = _mapper.Map<Category>(command.CategoryDto);
        category = await _categoryRepository.Add(category);

        response.Success = true;
        response.Message = "Creation Successful";
        response.Id = category.Id;

        return response;
    }
}