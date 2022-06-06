using MediatR;
using WebApi.Application.DTOs.Category;
using WebApi.Application.Response;

namespace WebApi.Application.Features.Categories.Requests.Commands;

public class CreateCategoryCommand : IRequest<BaseCommandResponse>
{
    public CreateCategoryDto CategoryDto { get; set; } = null!;
}