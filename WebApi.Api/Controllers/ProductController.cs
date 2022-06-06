using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.DTOs.Product;
using WebApi.Application.Features.Products.Requests.Commands;
using WebApi.Application.Features.Products.Requests.Queries;

namespace WebApi.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductListDto>>> Get()
        {
            var products = await _mediator.Send(new GetProductListRequest());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var product = await _mediator.Send(new GetProductDetailRequest { Id = id });
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateProductDto product)
        {
            var command = new CreateProductCommand { ProductDto = product };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateProductDto product)
        {
            var command = new UpdateProductCommand { Id = id, ProductDto = product };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteProductCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}