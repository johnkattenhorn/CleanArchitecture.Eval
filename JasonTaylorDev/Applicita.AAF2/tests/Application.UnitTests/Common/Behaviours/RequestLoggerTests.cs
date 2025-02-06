using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using Applicita.AAF2.Application.Common.Behaviours;
using Applicita.AAF2.Application.Common.Interfaces;
using Applicita.AAF2.Application.TodoItems.CreateTodoItem;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Applicita.AAF2.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateTodoItemCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateTodoItemCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand,int>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Handle(new CreateTodoItemCommand { ListId = 1, Title = "title" }, () => Task.FromResult(1), CancellationToken.None);

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand,int>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Handle(new CreateTodoItemCommand { ListId = 1, Title = "title" }, () => Task.FromResult(1), CancellationToken.None);

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
