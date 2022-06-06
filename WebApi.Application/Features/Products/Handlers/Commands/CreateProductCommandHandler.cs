using AutoMapper;
using MediatR;
using WebApi.Domain;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.DTOs.Product.Validators;
using WebApi.Application.Features.Products.Requests.Commands;
using WebApi.Application.Response;

namespace WebApi.Application.Features.Products.Handlers.Commands
{

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, BaseCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateProductDtoValidator(_productRepository);
            var validationResult = await validator.ValidateAsync(command.ProductDto, cancellationToken);
            if (validationResult is {IsValid: false})
            {
                response.Success = false;
                response.Message = "Creating failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
            }

            var product = _mapper.Map<Product>(command.ProductDto);
            product = await _productRepository.Add(product);

            response.Success = true;
            response.Message = "Creation Successful";
            response.Id = product.Id;

            return response;
        }
    }
}