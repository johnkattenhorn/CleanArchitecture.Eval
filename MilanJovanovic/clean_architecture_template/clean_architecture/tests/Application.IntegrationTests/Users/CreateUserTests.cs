using Application.IntegrationTests.Infrastructure;
using Application.Users.Create;
using Domain.Users;
using FluentAssertions;
using SharedKernel;

namespace Application.IntegrationTests.Users;

public class CreateUserTests : BaseIntegrationTest
{
    public CreateUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Handle_Should_CreateUser_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateUserCommand(Faker.Internet.Email(), Faker.Internet.UserName(), true);

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_AddUserToDatabase_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateUserCommand(Faker.Internet.Email(), Faker.Internet.UserName(), true);

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        User? user = await DbContext.Users.FindAsync(result.Value);

        user.Should().NotBeNull();
    }
}
