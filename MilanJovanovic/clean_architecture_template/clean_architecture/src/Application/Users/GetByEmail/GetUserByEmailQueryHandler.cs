using System.Data;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Users;
using SharedKernel;

namespace Application.Users.GetByEmail;

internal sealed class GetUserByEmailQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT
                u.id AS Id,
                u.email AS Email,
                u.name AS Name,
                u.has_public_profile AS HasPublicProfile
            FROM users u
            WHERE u.id = @Email
            """;

        using IDbConnection connection = factory.GetOpenConnection();

        UserResponse? user = await connection.QueryFirstOrDefaultAsync<UserResponse>(
            sql,
            query);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
        }

        return user;
    }
}
