using AutoMapper;
using MediatR;
using WebApi.Application.Contracts.Persistence;
using WebApi.Application.Exceptions;
using WebApi.Application.Features.Products.Requests.Commands;

namespace WebApi.Application.Features.Products.Handlers.Commands
{

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.Get(command.Id);
            if (product == null)
                throw new NotFoundException(nameof(product), command.Id);
            await _productRepository.Delete(product);
            return Unit.Value;
        }
    }
}