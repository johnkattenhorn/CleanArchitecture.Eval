namespace Application.Users;

public static class UserErrorCodes
{
    public static class CreateUser
    {
        public const string MissingName = nameof(MissingName);
        public const string MissingEmail = nameof(MissingEmail);
        public const string InvalidEmail = nameof(InvalidEmail);
    }
}
