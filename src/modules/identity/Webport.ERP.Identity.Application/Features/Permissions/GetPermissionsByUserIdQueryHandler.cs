using Dapper;

namespace Webport.ERP.Identity.Application.Features.Permissions;

public class GetPermissionsByUserIdQueryHandler(IDbConnectionFactory dbConnection)
    : IQueryHandler<GetPermissionsByUserIdQuery, GetPermissionsByUserIdQueryResult>
{
    public async Task<Result<GetPermissionsByUserIdQueryResult>> Handle(
        GetPermissionsByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        string sql =
        $"""
		    SELECT DISTINCT 
				   u.user_id as "{nameof(UserPermission.UserId)}",
				   p.permission_code as "{nameof(UserPermission.Permission)}" 
		    FROM identity.users u
		    LEFT JOIN identity.role_permissions rp 
			    ON rp.role_id = u.role_id
		    LEFT JOIN identity.permissions p 
			    ON p.permission_id = rp.permission_id
		    WHERE u.user_id = @UserId
		""";

        List<UserPermission> permissions = (await dbConnection.QueryAsync<UserPermission>(sql, new { query.UserId })).AsList();

        if (permissions.Count == 0)
        {
            return Result.Failure<GetPermissionsByUserIdQueryResult>(CustomError.NotFound("404", "No permissions found."));
        }

        var response = new GetPermissionsByUserIdQueryResult(
            new PermissionsResponse(permissions[0].UserId, [.. permissions.Select(p => p.Permission)]));

        return Result.Success(response);
    }
}

public sealed record GetPermissionsByUserIdQuery(int UserId) : IQuery<GetPermissionsByUserIdQuery>;

public sealed record GetPermissionsByUserIdQueryResult(PermissionsResponse Permissions);


internal sealed class UserPermission
{
    internal int UserId { get; init; }

    internal string Permission { get; init; } = string.Empty;
}