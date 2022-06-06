namespace WebApi.Application.DTOs.Product
{

    public interface IProductDto
    {
        public string? Sku { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
    }
}