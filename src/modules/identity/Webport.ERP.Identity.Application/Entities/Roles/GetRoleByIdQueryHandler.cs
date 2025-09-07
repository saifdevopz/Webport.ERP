using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Entities.Roles;

public class GetRoleByIdQueryHandler(IIdentityRepository<RoleM> repository)
    : IQueryHandler<GetRoleByIdQuery, GetRoleByIdQueryResult>
{
    public async Task<Result<GetRoleByIdQueryResult>> Handle(
        GetRoleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.RoleId == query.RoleId, cancellationToken);

        return model is not null
            ? Result.Success(new GetRoleByIdQueryResult(model))
            : Result.Failure<GetRoleByIdQueryResult>(CustomError.NotFound("Not Found", "Role not found."));
    }
}

public sealed record GetRoleByIdQuery(int RoleId) : IQuery<GetRoleByIdQuery>;

public sealed record GetRoleByIdQueryResult(RoleM Role);
