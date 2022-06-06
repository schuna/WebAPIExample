using AutoMapper;
using MediatR;
using Microsoft.VisualBasic;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.DTOs.Category;
using WebApi.Application.Features.Categories.Requests.Queries;

namespace WebApi.Application.Features.Categories.Handlers.Queries;

public class GetCategoryDetailRequestHandler : IRequestHandler<GetCategoryDetailRequest, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoryDetailRequestHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryDetailRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id);
        return _mapper.Map<CategoryDto>(category);
    }
}