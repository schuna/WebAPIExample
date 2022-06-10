using AutoMapper;
using MediatR;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.Exceptions;
using WebApi.Application.Features.Categories.Requests.Commands;

namespace WebApi.Application.Features.Categories.Handlers.Commands;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private IMapper Mapper { get; }
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        Mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(command.Id);
        if (category == null)
            throw new NotFoundException(nameof(category), command.Id);
        await _categoryRepository.Delete(category);
        return Unit.Value;
    }
}