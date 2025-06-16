using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BLL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class GewichtIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GewichtIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        // Arrange login
        var loginModel = new { Username = "TestUser", Password = "TestUser123!" };
        var loginResponse = _client.PostAsJsonAsync("/api/account/login", loginModel).Result;
        loginResponse.EnsureSuccessStatusCode();

        var loginResult = loginResponse.Content.ReadFromJsonAsync<LoginResponse>().Result;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);
    }
    
    //Gewicht testen

    [Fact]
    public async Task Get_AllGewichten_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/gewicht/gewicht");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_GewichtById_ShouldReturnOk()
    {
        // Arrange
        var id = 162;

        // Act
        var response = await _client.GetAsync($"/api/gewicht/gewicht/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_GewichtByInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = 999999;

        // Act
        var response = await _client.GetAsync($"/api/gewicht/gewicht/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_Gewicht_ShouldReturnOk()
    {
        // Arrange
        var model = new AddGewichtModel { Gewicht = 78 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/gewicht/gewicht", model);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_InvalidGewicht_ShouldReturnBadRequest()
    {
        // Arrange
        var model = new AddGewichtModel { Gewicht = -10 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/gewicht/gewicht", model);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Put_ValidGewicht_ShouldReturnOk()
    {
        // Arrange
        var addModel = new AddGewichtModel { Gewicht = 70 };
        var postResponse = await _client.PostAsJsonAsync("/api/gewicht/gewicht", addModel);
        postResponse.EnsureSuccessStatusCode();
        
        var gewichtenlijst = await _client.GetFromJsonAsync<List<GewichtModel>>("/api/gewicht/gewicht");
        var laatste = gewichtenlijst.Last();
        var id = laatste.id;

        var editModel = new EditGewichtModel
        {
            Id = id,
            Gewicht = 74
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/gewicht/gewicht/{id}", editModel);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Put_IdMismatch_ShouldReturnBadRequest()
    {
        // Arrange
        var model = new EditGewichtModel { Id = 1, Gewicht = 70 };

        // Act
        var response = await _client.PutAsJsonAsync("/api/gewicht/gewicht/999", model);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Delete_ValidGewicht_ShouldReturnOk()
    {
        // Arrange
        var newModel = new AddGewichtModel { Gewicht = 82 };
        var postResponse = await _client.PostAsJsonAsync("/api/gewicht/gewicht", newModel);
        postResponse.EnsureSuccessStatusCode();

        var gewichtenlijst = await _client.GetFromJsonAsync<List<GewichtModel>>("/api/gewicht/gewicht");
        var last = gewichtenlijst.Last();
        var id = last.id;

        // Act
        var response = await _client.DeleteAsync($"/api/gewicht/gewicht/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    
    
    // Doelgewicht testen
    
    
    [Fact]
    public async Task Get_AllDoelGewichten_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/gewicht/doelgewicht");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_DoelGewicht_ShouldReturnOk()
    {
        // Arrange
        var model = new AddDoelGewichtModel { Doelgewicht = 78 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/gewicht/doelgewicht", model);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_InvalidDoelGewicht_ShouldReturnBadRequest()
    {
        // Arrange
        var model = new AddDoelGewichtModel { Doelgewicht = -10 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/gewicht/doelgewicht", model);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Put_ValidDoelGewicht_ShouldReturnOk()
    {
        // Arrange
        var addModel = new AddDoelGewichtModel { Doelgewicht = 70 };
        var postResponse = await _client.PostAsJsonAsync("/api/gewicht/doelgewicht", addModel);
        postResponse.EnsureSuccessStatusCode();
        
        var doelgewichtenlijst = await _client.GetFromJsonAsync<List<DoelGewichtModel>>("/api/gewicht/doelgewicht");
        var laatste = doelgewichtenlijst.Last();
        var id = laatste.id;

        var editModel = new EditDoelGewichtModel
        {
            Id = id,
            Doelgewicht = 74
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/gewicht/doelgewicht/{id}", editModel);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Put_IdMismatchDoelGewicht_ShouldReturnBadRequest()
    {
        // Arrange
        var model = new EditDoelGewichtModel { Id = 1, Doelgewicht = 70 };

        // Act
        var response = await _client.PutAsJsonAsync("/api/gewicht/doelgewicht/999", model);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Delete_DoelGewicht_ShouldReturnOk()
    {
        // Arrange
        var newModel = new AddDoelGewichtModel { Doelgewicht = 82 };
        var postResponse = await _client.PostAsJsonAsync("/api/gewicht/doelgewicht", newModel);
        postResponse.EnsureSuccessStatusCode();

        var doelgewichtenlijst = await _client.GetFromJsonAsync<List<DoelGewichtModel>>("/api/gewicht/doelgewicht");
        var last = doelgewichtenlijst.Last();
        var id = last.id;

        // Act
        var response = await _client.DeleteAsync($"/api/gewicht/doelgewicht/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    

    private class LoginResponse
    {
        public string Token { get; set; }
    }
}
