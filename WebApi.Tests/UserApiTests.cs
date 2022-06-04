using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApi.DataAccess.DTOs.User;
using WebApi.DataAccess.Helpers;
using WebApi.DataAccess.Models;

namespace WebApi;

public class UserApiTests
{
    private WebApplicationFactory<Program> _application;
    private HttpClient _client;

    public UserApiTests()
    {
        _application = new ShopApiApplication();
        _client = _application.CreateClient();
    }

    [SetUp]
    public async Task SetUp()
    {
        await SetInMemoryDb();
    }

    [Test]
    public async Task GetUsers_WhenCalledSuccessful_ReturnUserList()
    {
        var response = await _client.GetFromJsonAsync<List<User>>("/api/user");
        Assert.IsNotNull(response);
        Assert.That(response!.Count, Is.EqualTo(3));
        Assert.That(response[0].Email, Is.EqualTo("smith.sam@abc.com"));
        Assert.That(response[1].LastName, Is.EqualTo("Mac"));
        Assert.That(response[2].Id, Is.EqualTo(3));
    }

    [Test]
    public async Task GetUser_WhenCalledSuccessful_ReturnStatus()
    {
        var response = await _client.GetAsync("/api/user/1");
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetUser_WhenEntryNotFound_ReturnNotFound()
    {
        var response = await _client.GetAsync("api/user/900");
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task PostUser_WhenCalledSuccessful_ReturnEntryList()
    {
        var user = new UserCreateDto
        {
            FirstName = "storm",
            LastName = "Sonic",
            Email = "storm.sonic@abc.com",
            Password = "nd0rmP0sSw0d!",
            ConfirmPassword = "nd0rmP0sSw0d!"
        };
        var response = await _client.PostAsJsonAsync($"/api/user", user);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task PostUser_WhenEmailAlreadyExist_ReturnBadRequest()
    {
        var user = new UserCreateDto
        {
            FirstName = "smith.sam",
            LastName = "max",
            Email = "smith.sam@abc.com",
            Password = "jsfh$#23adsfg@",
            ConfirmPassword = "jsfh$#23adsfg@"
        };
        var response = await _client.PostAsJsonAsync($"/api/user", user);
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [TestCase(1, true, HttpStatusCode.NoContent)]
    [TestCase(900, false, HttpStatusCode.NotFound)]
    [TestCase(2, false, HttpStatusCode.BadRequest)]
    public async Task PutUser_WhenCalled_ReturnStatus(
        int id, 
        bool successStatusCode, 
        HttpStatusCode expected)
    {
        var user = new UserUpdateDto
        {
            FirstName = "Alex",
            LastName = "Max",
            Email = "smith.sam@abc.com",
            Password = "Kj908$@fdf@4",
            ConfirmPassword = "Kj908$@fdf@4"
        };
        var response = await _client.PutAsJsonAsync($"/api/user/{id}", user);
        Assert.That(response.IsSuccessStatusCode, Is.EqualTo(successStatusCode));
        Assert.That(response.StatusCode, Is.EqualTo(expected));
    }

    [Test]
    public async Task DeleteUser_WhenCalledSuccessful_ReturnDeletedItem()
    {
        var response = await _client.DeleteAsync($"/api/user/1");
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
    
    [Test]
    public async Task DeleteUser_WhenEntryNotExists_ReturnNotFound()
    {
        var response = await _client.DeleteAsync("/api/user/900");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private async Task SetInMemoryDb()
    {
        using (var scope = _application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var shopContext = provider.GetRequiredService<ShopContext>())
            {
                await shopContext.Database.EnsureCreatedAsync();
                await shopContext.Database.EnsureDeletedAsync();
                await shopContext.Users.AddAsync(
                    new User
                    {
                        Id = 1,
                        FirstName = "Smith",
                        LastName = "Sam",
                        Email = "smith.sam@abc.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("d@sdf0rd4435df!")
                    }
                    );
                await shopContext.Users.AddAsync(
                    new User
                    {
                        Id = 2,
                        FirstName = "Almond",
                        LastName = "Mac",
                        Email = "almond.mac@abc.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("daf@#$dga3dGH")
                    }
                );
                await shopContext.Users.AddAsync(
                    new User
                    {
                        Id = 3,
                        FirstName = "Jay",
                        LastName = "blue",
                        Email = "jay.blue@abc.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("dd#5dHs3DFHG")
                    }
                );
                await shopContext.SaveChangesAsync();
            }
        }
    }
}