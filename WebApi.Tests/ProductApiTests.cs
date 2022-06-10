using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using WebApi.Application.DTOs.Product;
using WebApi.Domain;
using WebApi.Domain.ViewModel;
using WebApi.Persistence;

namespace WebApi.Api;

public class ProductApiTests
{
    private readonly WebApplicationFactory<Program> _application;
    private readonly HttpClient _client;
    private AuthResultViewModel _authResultViewModel = null!;
    private Product? _product;
    private CreateProductDto _createProductDto = null!;
    private WebApiDbContext _webApiDbContext = null!;
    private Product? _productCreated;

    public ProductApiTests()
    {
        _application = new WebApplicationFactory<Program>();
        _client = _application.CreateClient();
    }

    [OneTimeSetUp]
    public async Task Setup()
    {
        await GetAccessToken();
        await SetUpDatabase();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await TearDownDatabase();
    }

    [Test]
    public async Task GetAllProducts_WhenCalledSuccessful_ReturnEntryList()
    {
        var response = await _client.GetFromJsonAsync<List<ProductListDto>>("/api/Product");
        Assert.IsNotNull(response);
        Assert.IsTrue(response!.Count != 0);
    }

    [Test]
    public async Task GetProductById_WhenSuccessful_ReturnEntry()
    {
        var response = await _client.GetFromJsonAsync<ProductDto>(
            $"/api/product/{_product!.Id}");
        Assert.That(response!.Id, Is.EqualTo(_product.Id));
        Assert.That(response.Name, Is.EqualTo(_product.Name));
        Assert.That(response.CategoryId, Is.EqualTo(_product.CategoryId));
    }

    [Test]
    public async Task PostProduct_WhenSuccessful_ReturnOk()
    {
        _createProductDto = new CreateProductDto
        {
            CategoryId = 1,
            Description = "Summer cool",
            IsAvailable = true,
            Name = "Cool Jacket",
            Price = 560,
            Sku = "DAGS"
        };
        var response = await _client.PostAsJsonAsync("/api/Product", _createProductDto);
        response.EnsureSuccessStatusCode();
        _productCreated = await response.Content.ReadFromJsonAsync<Product>();
        Assert.IsTrue(response.IsSuccessStatusCode);
    }

    [Test]
    public async Task PutProduct_WhenSuccessful_ReturnNoContent()
    {
        var productUpdate = new UpdateProductDto
        {
            Id = _product!.Id,
            CategoryId = _product.CategoryId,
            Description = _product.Description,
            IsAvailable = _product.IsAvailable,
            Name = _product.Name,
            Price = _product.Price + 100,
            Sku = _product.Sku
        };

        var response = await _client.PutAsJsonAsync($"/api/Product/{_product!.Id}", productUpdate);
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    private async Task GetAccessToken()
    {
        TokenRequestViewModel tokenRequest;
        using (var reader = new StreamReader("config.json"))
        {
            var json = await reader.ReadToEndAsync();
            tokenRequest = JsonConvert.DeserializeObject<TokenRequestViewModel>(json)!;
        }

        var response = await _client.PostAsJsonAsync("/api/Authentication/refresh-token", tokenRequest);
        response.EnsureSuccessStatusCode();
        _authResultViewModel = (await response.Content.ReadFromJsonAsync<AuthResultViewModel>())!;
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _authResultViewModel.Token);
    }

    private async Task SetUpDatabase()
    {
        using var scope = _application.Services.CreateScope();
        var provider = scope.ServiceProvider;
        _webApiDbContext = provider.GetRequiredService<WebApiDbContext>();
        await _webApiDbContext.Products.AddAsync(
            new Product
            {
                Description = "Winter Sports Wear",
                CategoryId = 1,
                Name = "Grunge Skater Jeans",
                Sku = "AWMGSJ",
                Price = 68,
                IsAvailable = true
            });
        await _webApiDbContext.SaveChangesAsync();
        var products = await _webApiDbContext.Products.ToListAsync();
        _product = products.LastOrDefault()!;
    }

    private async Task TearDownDatabase()
    {
        using var scope = _application.Services.CreateScope();
        var provider = scope.ServiceProvider;
        await using var webApiDbContext = provider.GetRequiredService<WebApiDbContext>();
        webApiDbContext.Products.Remove(_product!);
        webApiDbContext.Products.Remove(_productCreated!);
        await webApiDbContext.SaveChangesAsync();
    }
}