using MediatR;
using WebApi.Application.DTOs.Product;

namespace WebApi.Application.Features.Products.Requests.Queries
{

    public class GetProductDetailRequest : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }
}