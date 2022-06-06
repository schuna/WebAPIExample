using MediatR;
using WebApi.Application.DTOs.Category;

namespace WebApi.Application.Features.Categories.Requests.Queries;

public class GetCategoryDetailRequest : IRequest<CategoryDto>
{
    public int Id { get; set; }
}