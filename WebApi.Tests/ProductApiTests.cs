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
using WebApi.Application.DTOs.Category;
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
    private Category? _category;
    private CreateCategoryDto _createCategoryDto = null!;
    private Category _categoryCreated = null!;
    private RegisterViewModel _registeredUser = null!;

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
        await RegisterUser();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await TearDownDatabase();
    }

    [Test]
    public async Task GetAllProducts_WhenCalledSuccessful_ReturnEntryList()
    {
        var response = await _client.GetFromJsonAsync<List<ProductListDto>>("api/Product");
        Assert.IsNotNull(response);
        Assert.IsTrue(response!.Count != 0);
    }

    [Test]
    public async Task GetAllCategories_WhenSuccessful_ReturnEntries()
    {
        var respose = await _client.GetFromJsonAsync<List<CategoryListDto>>("api/Category");
        Assert.That(respose!.Count, Is.Not.Zero);
        Assert.That(respose.LastOrDefault()!.Id, Is.EqualTo(_category!.Id));
        Assert.That(respose.LastOrDefault()!.Name, Is.EqualTo(_category!.Name));
    }

    [Test]
    public async Task GetProductById_WhenSuccessful_ReturnEntry()
    {
        var response = await _client.GetFromJsonAsync<ProductDto>(
            $"api/product/{_product!.Id}");
        Assert.That(response!.Id, Is.EqualTo(_product.Id));
        Assert.That(response.Name, Is.EqualTo(_product.Name));
        Assert.That(response.CategoryId, Is.EqualTo(_product.CategoryId));
    }

    [Test]
    public async Task GetCategoryById_WhenSuccessful_ReturnEntry()
    {
        var response = await _client.GetFromJsonAsync<CategoryDto>($"api/Category/{_category!.Id}");
        Assert.That(response!.Id, Is.EqualTo(_category.Id));
        Assert.That(response.Name, Is.EqualTo(_category.Name));
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
        var response = await _client.PostAsJsonAsync("api/Product", _createProductDto);
        response.EnsureSuccessStatusCode();
        _productCreated = await response.Content.ReadFromJsonAsync<Product>();
        Assert.IsTrue(response.IsSuccessStatusCode);
    }

    [Test]
    public async Task PostCategory_WhenSuccessful_ReturnOk()
    {
        _createCategoryDto = new CreateCategoryDto
        {
            Name = "Sweater"
        };
        var response = await _client.PostAsJsonAsync("/api/Category", _createCategoryDto);
        response.EnsureSuccessStatusCode();
        _categoryCreated = (await response.Content.ReadFromJsonAsync<Category>())!;
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

        var response = await _client.PutAsJsonAsync($"api/Product/{_product!.Id}", productUpdate);
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task PutCategory_WhenSuccessful_ReturnNoContent()
    {
        var updateCategoryDto = new UpdateCategoryDto
        {
            Id = _category!.Id,
            Name = "Sports Wear"
        };
        var response = await _client.PutAsJsonAsync($"api/Category/{_category!.Id}", updateCategoryDto);
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task Login_WhenSuccessful_ReturnOk()
    {
        var loginViewModel = new LoginViewModel
        {
            EmailAddress = _registeredUser.EmailAddress,
            Password = _registeredUser.Password
        };
        var response = await _client.PostAsJsonAsync("api/Authentication/login-user", loginViewModel);
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task GetAccessToken()
    {
        TokenRequestViewModel tokenRequest;
        using (var reader = new StreamReader("config.json"))
        {
            var json = await reader.ReadToEndAsync();
            tokenRequest = JsonConvert.DeserializeObject<TokenRequestViewModel>(json)!;
        }

        var response = await _client.PostAsJsonAsync("api/Authentication/refresh-token", tokenRequest);
        response.EnsureSuccessStatusCode();
        _authResultViewModel = (await response.Content.ReadFromJsonAsync<AuthResultViewModel>())!;
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _authResultViewModel.Token);
    }

    private async Task SetUpDatabase()
    {
        await InitCategoryDb();
        await InitProductDb();
    }

    private async Task InitProductDb()
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

    private async Task InitCategoryDb()
    {
        using var scope = _application.Services.CreateScope();
        var provider = scope.ServiceProvider;
        _webApiDbContext = provider.GetRequiredService<WebApiDbContext>();
        await _webApiDbContext.Categories.AddAsync(
            new Category
            {
                Id = 2,
                Name = "Sports Wear"
            }
        );
        await _webApiDbContext.SaveChangesAsync();
        var categories = await _webApiDbContext.Categories.ToListAsync();
        _category = categories.LastOrDefault();
    }

    private async Task RegisterUser()
    {
        try
        {
            _registeredUser = new RegisterViewModel
            {
                FirstName = "User",
                LastName = "Anonymous",
                EmailAddress = "User.Anonymous@amail.com",
                UserName = "UserAnonymous",
                Password = "P@sSw0rd!"
            };
            await _client.PostAsJsonAsync("api/Authentication/register-user", _registeredUser);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    private async Task TearDownDatabase()
    {
        using var scope = _application.Services.CreateScope();
        var provider = scope.ServiceProvider;
        await using var webApiDbContext = provider.GetRequiredService<WebApiDbContext>();
        await _client.DeleteAsync($"api/Product/{_product!.Id}");
        await _client.DeleteAsync($"api/Product/{_productCreated!.Id}");
        await _client.DeleteAsync($"api/Category/{_category!.Id}");
        await _client.DeleteAsync($"api/Category/{_categoryCreated!.Id}");
    }
}