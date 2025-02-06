using System.Net;
using System.Net.Http.Json;
using Api.FunctionalTests.Infrastructure;
using Application.Users.Create;
using Application.Users.GetById;
using FluentAssertions;

namespace Api.FunctionalTests.Users;

public class GetUserTests : BaseFunctionalTest
{
    public GetUserTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"api/v1/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        Guid userId = await CreateUserAsync();

        // Act
        UserResponse? user = await HttpClient.GetFromJsonAsync<UserResponse>($"api/v1/users/{userId}");

        // Assert
        user.Should().NotBeNull();
    }

    private async Task<Guid> CreateUserAsync()
    {
        var request = new CreateUserRequest("test@test.com", "name", true);

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}
