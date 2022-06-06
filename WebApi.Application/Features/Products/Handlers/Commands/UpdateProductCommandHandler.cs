using AutoMapper;
using MediatR;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.DTOs.Product.Validators;
using WebApi.Application.Exceptions;
using WebApi.Application.Features.Products.Requests.Commands;

namespace WebApi.Application.Features.Products.Handlers.Commands
{

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateProductDtoValidator(_productRepository);
            var validationResult = await validator.ValidateAsync(command.ProductDto, cancellationToken);

            if (validationResult is {IsValid: false})
                throw new ValidationException(validationResult);

            var product = await _productRepository.Get(command.Id);
            _mapper.Map(command.ProductDto, product);
            await _productRepository.Update(product);
            return Unit.Value;
        }
    }
}