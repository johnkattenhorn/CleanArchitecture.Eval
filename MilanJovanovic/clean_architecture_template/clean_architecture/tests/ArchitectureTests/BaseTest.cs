using System.Reflection;
using Application.Abstractions.Messaging;
using Domain.Users;
using Infrastructure.Database;
using Web.Api;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(User).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ICommand).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(DbConnectionFactory).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
