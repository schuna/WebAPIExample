using MediatR;

namespace WebApi.Application.Features.Products.Requests.Commands
{

    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }
}