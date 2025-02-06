namespace Bookify.Api.Controllers.Users;

public sealed record UpdateUserProfileRequest(
    Guid UserId,
    string FirstName,
    string LastName);
