using MediatR;
using WebApi.Application.DTOs.Product;
using WebApi.Application.Response;

namespace WebApi.Application.Features.Products.Requests.Commands
{

    public class CreateProductCommand : IRequest<BaseCommandResponse>
    {
        public CreateProductDto ProductDto { get; set; } = null!;
    }
}