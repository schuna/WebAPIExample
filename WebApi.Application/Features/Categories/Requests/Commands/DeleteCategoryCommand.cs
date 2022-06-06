using MediatR;

namespace WebApi.Application.Features.Categories.Requests.Commands;

public class DeleteCategoryCommand : IRequest
{
    public int Id { get; set; }
}