using System.Net;
using System.Net.Http.Json;
using Api.FunctionalTests.Contracts;
using Api.FunctionalTests.Extensions;
using Api.FunctionalTests.Infrastructure;
using Application.Users;
using Application.Users.Create;
using FluentAssertions;

namespace Api.FunctionalTests.Users;

public class CreateUserTests : BaseFunctionalTest
{
    public CreateUserTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenEmailIsMissing()
    {
        // Arrange
        var request = new CreateUserRequest("", "name", true);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([
                UserErrorCodes.CreateUser.MissingEmail,
                UserErrorCodes.CreateUser.InvalidEmail
            ]);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new CreateUserRequest("test", "name", true);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([UserErrorCodes.CreateUser.InvalidEmail]);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenNameIsMissing()
    {
        // Arrange
        var request = new CreateUserRequest("test@test.com", "", true);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        CustomProblemDetails problemDetails = await response.GetProblemDetails();

        problemDetails.Errors.Select(e => e.Code)
            .Should()
            .Contain([UserErrorCodes.CreateUser.MissingName]);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateUserRequest("test@test.com", "name", true);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnConflict_WhenUserExists()
    {
        // Arrange
        var request = new CreateUserRequest("test@test.com", "name", true);

        // Act
        await HttpClient.PostAsJsonAsync("api/v1/users", request);

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
