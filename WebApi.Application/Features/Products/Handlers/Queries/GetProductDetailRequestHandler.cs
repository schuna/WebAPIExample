using AutoMapper;
using MediatR;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.DTOs.Product;
using WebApi.Application.Features.Products.Requests.Queries;

namespace WebApi.Application.Features.Products.Handlers.Queries
{

    public class GetProductDetailRequestHandler : IRequestHandler<GetProductDetailRequest, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductDetailRequestHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductDetailRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductWithDetails(request.Id);
            return _mapper.Map<ProductDto>(product);
        }
    }
}