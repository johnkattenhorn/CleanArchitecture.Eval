namespace Application.Users.GetByEmail;

public sealed record UserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string Name { get; init; }

    public bool HasPublicProfile { get; init; }
}
