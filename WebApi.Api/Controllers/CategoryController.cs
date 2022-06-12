using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.DTOs.Category;
using WebApi.Application.Features.Categories.Requests.Commands;
using WebApi.Application.Features.Categories.Requests.Queries;
using WebApi.Domain.Models.Helpers;

namespace WebApi.Api.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryListDto>>> Get()
        {
            var products = await _mediator.Send(new GetCategoryListRequest());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            var product = await _mediator.Send(new GetCategoryDetailRequest() { Id = id });
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateCategoryDto category)
        {
            var command = new CreateCategoryCommand { CategoryDto = category };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateCategoryDto category)
        {
            var command = new UpdateCategoryCommand { Id = id, CategoryDto = category };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteCategoryCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}