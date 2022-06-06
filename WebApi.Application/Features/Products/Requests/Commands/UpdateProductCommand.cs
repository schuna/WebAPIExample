using MediatR;
using WebApi.Application.DTOs.Product;

namespace WebApi.Application.Features.Products.Requests.Commands
{

    public class UpdateProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public UpdateProductDto ProductDto { get; set; } = null!;
    }
}