using MediatR;
using WebApi.Application.DTOs.Category;

namespace WebApi.Application.Features.Categories.Requests.Queries;

public class GetCategoryListRequest : IRequest<List<CategoryListDto>>
{
    
}