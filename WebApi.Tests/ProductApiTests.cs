using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApi.Application.DTOs.Product;
using WebApi.Domain;
using WebApi.Persistence;

namespace WebApi.Api;

public class ProductApiTests
{
    private WebApplicationFactory<Program> _application;
    private HttpClient _client;

    public ProductApiTests()
    {
        _application = new WebApplicationFactory<Program>();
        _client = _application.CreateClient();
    }

    [SetUp]
    public async Task SetUp()
    {
        await SetInmemoryDb();
    }

    [Test]
    public async Task GetAllProducts_WhenCalledSuccessful_ReturnEntryList()
    {
        var response = await _client.GetFromJsonAsync<List<ProductListDto>>("/api/Product");
        Assert.IsNotNull(response);
        Assert.IsTrue(response!.Count != 0);
    }

    private async Task SetInmemoryDb()
    {
        using (var scope = _application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var webApiDbContext = provider.GetRequiredService<WebApiDbContext>())
            {
                await webApiDbContext.Database.EnsureCreatedAsync();

                await webApiDbContext.Products.AddAsync(new Product
                {
                    Description = "",
                    CategoryId = 1,
                    Name = "Grunge Skater Jeans",
                    Sku = "AWMGSJ",
                    Price = 68,
                    IsAvailable = true
                });
                await webApiDbContext.Products.AddAsync(new Product
                {
                    Description = "",
                    CategoryId = 1,
                    Name = "Polo Shirt",
                    Sku = "AWMPS",
                    Price = 35,
                    IsAvailable = true
                });
                await webApiDbContext.Products.AddAsync(
                    new Product
                    {
                        Description = "",
                        CategoryId = 1,
                        Name = "Skater Graphic T-Shirt",
                        Sku = "AWMSGT",
                        Price = 33,
                        IsAvailable = true
                    });
                await webApiDbContext.Products.AddAsync(
                    new Product
                    {
                        Description = "",
                        CategoryId = 1,
                        Name = "Slicker Jacket",
                        Sku = "AWMSJ",
                        Price = 125,
                        IsAvailable = true
                    });
                await webApiDbContext.Products.AddAsync(
                    new Product
                    {
                        Description = "",
                        CategoryId = 1,
                        Name = "Thermal Fleece Jacket",
                        Sku = "AWMTFJ",
                        Price = 60,
                        IsAvailable = true
                    });
                await webApiDbContext.Products.AddAsync(new Product
                {
                    Description = "",
                    CategoryId = 1,
                    Name = "Unisex Thermal Vest",
                    Sku = "AWMUTV",
                    Price = 95,
                    IsAvailable = true
                });
                await webApiDbContext.Products.AddAsync(new Product
                {
                    Description = "",
                    CategoryId = 1,
                    Name = "V-Neck Pullover",
                    Sku = "AWMVNP",
                    Price = 65,
                    IsAvailable = true
                });
                await webApiDbContext.Products.AddAsync(new Product
                {
                    Description = "",
                    CategoryId = 1,
                    Name = "V-Neck Sweater",
                    Sku = "AWMVNS",
                    Price = 65,
                    IsAvailable = true
                });
                await webApiDbContext.Products.AddAsync(
                    new Product
                    {
                        Description = "",
                        CategoryId = 1,
                        Name = "V-Neck T-Shirt",
                        Sku = "AWMVNT",
                        Price = 17,
                        IsAvailable = true
                    });
                await webApiDbContext.SaveChangesAsync();
                Console.WriteLine("saved");
            }
        }
    }
}