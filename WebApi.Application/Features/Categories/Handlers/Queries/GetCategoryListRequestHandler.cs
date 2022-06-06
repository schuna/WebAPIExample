using AutoMapper;
using MediatR;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.DTOs.Category;
using WebApi.Application.Features.Categories.Requests.Queries;

namespace WebApi.Application.Features.Categories.Handlers.Queries;

public class GetCategoryListRequestHandler : IRequestHandler<GetCategoryListRequest, List<CategoryListDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoryListRequestHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryListDto>> Handle(GetCategoryListRequest request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAll();
        return _mapper.Map<List<CategoryListDto>>(categories);
    }
}