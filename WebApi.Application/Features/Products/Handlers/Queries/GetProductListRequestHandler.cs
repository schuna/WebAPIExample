using AutoMapper;
using MediatR;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.DTOs.Product;
using WebApi.Application.Features.Products.Requests.Queries;

namespace WebApi.Application.Features.Products.Handlers.Queries
{

    public class GetProductListRequestHandler : IRequestHandler<GetProductListRequest, List<ProductListDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductListRequestHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductListDto>> Handle(GetProductListRequest request,
            CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsWithDetails();
            return _mapper.Map<List<ProductListDto>>(products);
        }
    }
}