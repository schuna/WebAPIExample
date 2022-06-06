using MediatR;
using WebApi.Application.DTOs.Product;

namespace WebApi.Application.Features.Products.Requests.Queries
{

    public class GetProductListRequest : IRequest<List<ProductListDto>>
    {

    }
}