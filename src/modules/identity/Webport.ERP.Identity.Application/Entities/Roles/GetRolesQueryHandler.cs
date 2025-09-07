using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Entities.Roles;

public class GetRolesQueryHandler(IIdentityRepository<RoleM> repository)
    : IQueryHandler<GetRolesQuery, GetRolesQueryResult>
{
    public async Task<Result<GetRolesQueryResult>> Handle(
        GetRolesQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetRolesQueryResult(model));
    }
}

public sealed record GetRolesQuery : IQuery<GetRolesQueryResult>;

public sealed record GetRolesQueryResult(IEnumerable<RoleM> Roles);