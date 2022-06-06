using MediatR;
using WebApi.Application.DTOs.Category;

namespace WebApi.Application.Features.Categories.Requests.Commands;

public class UpdateCategoryCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public UpdateCategoryDto CategoryDto { get; set; } = null!;
}