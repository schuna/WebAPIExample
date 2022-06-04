using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApi.DataAccess.Helpers;
using WebApi.DataAccess.Models;


namespace WebApi;

public class ProductApiTests
{
    private WebApplicationFactory<Program> _application;
    private HttpClient _client;

    public ProductApiTests()
    {
        _application = new ShopApiApplication();
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
        var response = await _client.GetFromJsonAsync<List<Product>>("/products");
        Assert.IsNotNull(response);
        Assert.IsTrue(response!.Count == 33);
    }

    [Test]
    [TestCase(@"MinPrice=50&MaxPrice=100", 9)]
    [TestCase(@"SearchTerm=Coat", 1)]
    [TestCase(@"Sku=AWWBTSC", 1)]
    [TestCase(@"Name=Bamboo Thermal Ski Coat", 1)]
    [TestCase(@"", 33)]
    [TestCase(@"Page=1", 33)]
    [TestCase(@"Size=20", 33)]
    [TestCase(@"SortBy=Name&SortOrder=asc", 33)]
    public async Task GetAllProducts_WhenCalledWithParameter_ReturnFilteredResult(string parameters, int expectedCount)
    {
        var response = await _client.GetFromJsonAsync<List<Product>>($@"/products?{parameters}");
        Console.WriteLine(response!.Count);
        Assert.IsNotNull(response);
        Assert.IsTrue(response!.Count == expectedCount);
    }

    [Test]
    public async Task GetProduct_WhenCalledSuccessful_ReturnStatus()
    {
        var response = await _client.GetAsync("/products/6");
        Assert.IsNotNull(response);
        Assert.IsTrue(response.IsSuccessStatusCode);
    }

    [Test]
    public async Task GetProduct_WhenEntryNotFound_ReturnNotFound()
    {
        var response = await _client.GetAsync("/products/41");
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task PostProduct_WhenCalledSuccessful_ReturnEntryList()
    {
        var product = new Product
        {
            Id = 34,
            Description = "Brand New",
            CategoryId = 6,
            Name = "Cool Jacket",
            Sku = "CWMTFJ",
            Price = 100,
            IsAvailable = true
        };
        var response = await _client.PostAsJsonAsync<Product>($"/products", product);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    [TestCase(6, true, HttpStatusCode.NoContent)]
    [TestCase(900, false, HttpStatusCode.NotFound)]
    [TestCase(1000, false, HttpStatusCode.BadRequest)]
    
    public async Task PutProduct_WhenCalledSuccessful_ReturnStatus(
        int id,
        bool successStatusCode, 
        HttpStatusCode expected)
    {
        var product = new Product
        {
            Id = id % 999,
            Description = "Promotion",
            CategoryId = 1,
            Name = "Thermal Fleece Jacket",
            Sku = "AWMTFJ",
            Price = 54,
            IsAvailable = true
        };
        var response = await _client.PutAsJsonAsync($"/products/{id}", product);
        Assert.That(response.IsSuccessStatusCode, Is.EqualTo(successStatusCode));
        Assert.That(response.StatusCode, Is.EqualTo(expected));
    }

    [Test]
    public async Task DeleteProduct_WhenCalledSuccessful_ReturnDeletedItem()
    {
        var response = await _client.DeleteAsync($"/products/5");
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteProduct_WhenEntryNotExists_ReturnNotFound()
    {
        var response = await _client.DeleteAsync("/products/900");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task DeleteProducts_WhenCalledSuccessful_ReturnDeletedItems()
    {
        var response = await _client.PostAsync($"/products/Delete?ids=3&ids=4", null);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteProducts_WhenEntryNotExists_ReturnNotFound()
    {
        var response = await _client.PostAsync($"/products/Delete?ids=3&ids=900", null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private async Task SetInmemoryDb()
    {
        using (var scope = _application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var shopContext = provider.GetRequiredService<ShopContext>())
            {
                await shopContext.Database.EnsureCreatedAsync();
                await shopContext.Database.EnsureDeletedAsync();

                await shopContext.Products.AddAsync(new Product
                {
                    Id = 1,
                    Description = "",
                    CategoryId = 1,
                    Name = "Grunge Skater Jeans",
                    Sku = "AWMGSJ",
                    Price = 68,
                    IsAvailable = true
                });
                await shopContext.Products.AddAsync(new Product
                {
                    Id = 2,
                    Description = "",
                    CategoryId = 1,
                    Name = "Polo Shirt",
                    Sku = "AWMPS",
                    Price = 35,
                    IsAvailable = true
                });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 3,
                        Description = "",
                        CategoryId = 1,
                        Name = "Skater Graphic T-Shirt",
                        Sku = "AWMSGT",
                        Price = 33,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 4,
                        Description = "",
                        CategoryId = 1,
                        Name = "Slicker Jacket",
                        Sku = "AWMSJ",
                        Price = 125,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 5,
                        Description = "",
                        CategoryId = 1,
                        Name = "Thermal Fleece Jacket",
                        Sku = "AWMTFJ",
                        Price = 60,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(new Product
                {
                    Id = 6,
                    Description = "",
                    CategoryId = 1,
                    Name = "Unisex Thermal Vest",
                    Sku = "AWMUTV",
                    Price = 95,
                    IsAvailable = true
                });
                await shopContext.Products.AddAsync(new Product
                {
                    Id = 7,
                    Description = "",
                    CategoryId = 1,
                    Name = "V-Neck Pullover",
                    Sku = "AWMVNP",
                    Price = 65,
                    IsAvailable = true
                });
                await shopContext.Products.AddAsync(new Product
                {
                    Id = 8,
                    Description = "",
                    CategoryId = 1,
                    Name = "V-Neck Sweater",
                    Sku = "AWMVNS",
                    Price = 65,
                    IsAvailable = true
                });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 9,
                        Description = "",
                        CategoryId = 1,
                        Name = "V-Neck T-Shirt",
                        Sku = "AWMVNT",
                        Price = 17,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 10,
                        Description = "",
                        CategoryId = 2,
                        Name = "Bamboo Thermal Ski Coat",
                        Sku = "AWWBTSC",
                        Price = 99,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 11,
                        Description = "",
                        CategoryId = 2,
                        Name = "Cross-Back Training Tank",
                        Sku = "AWWCTT",
                        Price = 0,
                        IsAvailable = false
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 12,
                        Description = "",
                        CategoryId = 2,
                        Name = "Grunge Skater Jeans",
                        Sku = "AWWGSJ",
                        Price = 68,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 13,
                        Description = "",
                        CategoryId = 2,
                        Name = "Slicker Jacket",
                        Sku = "AWWSJ",
                        Price = 125,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 14,
                        Description = "",
                        CategoryId = 2,
                        Name = "Stretchy Dance Pants",
                        Sku = "AWWSDP",
                        Price = 55,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 15,
                        Description = "",
                        CategoryId = 2,
                        Name = "Ultra-Soft Tank Top",
                        Sku = "AWWUTT",
                        Price = 22,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 16,
                        Description = "",
                        CategoryId = 2,
                        Name = "Unisex Thermal Vest",
                        Sku = "AWWUTV",
                        Price = 95,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 17,
                        Description = "",
                        CategoryId = 2,
                        Name = "V-Next T-Shirt",
                        Sku = "AWWVNT",
                        Price = 17,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 18,
                        Description = "",
                        CategoryId = 3,
                        Name = "Blueberry Mineral Water",
                        Sku = "MWB",
                        Price = 2.8M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 19,
                        Description = "",
                        CategoryId = 3,
                        Name = "Lemon-Lime Mineral Water",
                        Sku = "MWLL",
                        Price = 2.8M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 20,
                        Description = "",
                        CategoryId = 3,
                        Name = "Orange Mineral Water",
                        Sku = "MWO",
                        Price = 2.8M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 21,
                        Description = "",
                        CategoryId = 3,
                        Name = "Peach Mineral Water",
                        Sku = "MWP",
                        Price = 2.8M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 22,
                        Description = "",
                        CategoryId = 3,
                        Name = "Raspberry Mineral Water",
                        Sku = "MWR",
                        Price = 2.8M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 23,
                        Description = "",
                        CategoryId = 3,
                        Name = "Strawberry Mineral Water",
                        Sku = "MWS",
                        Price = 2.8M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 24,
                        Description = "",
                        CategoryId = 4,
                        Name = "In the Kitchen with H+ Sport",
                        Sku = "PITK",
                        Price = 24.99M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 25,
                        Description = "",
                        CategoryId = 5,
                        Name = "Calcium 400 IU (150 tablets)",
                        Sku = "SC400",
                        Price = 9.99M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 26,
                        Description = "",
                        CategoryId = 5,
                        Name = "Flaxseed Oil 100 mg (90 capsules)",
                        Sku = "SFO100",
                        Price = 12.49M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 27,
                        Description = "",
                        CategoryId = 5,
                        Name = "Iron 65 mg (150 caplets)",
                        Sku = "SI65",
                        Price = 13.99M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 28,
                        Description = "",
                        CategoryId = 5,
                        Name = "Magnesium 250 mg (100 tablets)",
                        Sku = "SM250",
                        Price = 12.49M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 29,
                        Description = "",
                        CategoryId = 5,
                        Name = "Multi-Vitamin (90 capsules)",
                        Sku = "SMV",
                        Price = 9.99M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 30,
                        Description = "",
                        CategoryId = 5,
                        Name = "Vitamin A 10,000 IU (125 caplets)",
                        Sku = "SVA",
                        Price = 11.99M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 31,
                        Description = "",
                        CategoryId = 5,
                        Name = "Vitamin B-Complex (100 caplets)",
                        Sku = "SVB",
                        Price = 12.99M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 32,
                        Description = "",
                        CategoryId = 5,
                        Name = "Vitamin C 1000 mg (100 tablets)",
                        Sku = "SVC",
                        Price = 9.99M,
                        IsAvailable = true
                    });
                await shopContext.Products.AddAsync(
                    new Product
                    {
                        Id = 33,
                        Description = "",
                        CategoryId = 5,
                        Name = "Vitamin D3 1000 IU (100 tablets)",
                        Sku = "SVD3",
                        Price = 12.49M,
                        IsAvailable = true
                    });
                await shopContext.SaveChangesAsync();
            }
        }
    }
}