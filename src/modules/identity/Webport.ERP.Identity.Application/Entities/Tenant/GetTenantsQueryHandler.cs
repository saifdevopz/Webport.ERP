using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Entities.Tenant;

internal sealed class GetTenantsQueryHandler(IIdentityRepository<TenantM> repository)
    : IQueryHandler<GetTenantsQuery, GetTenantsQueryResult>
{
    public async Task<Result<GetTenantsQueryResult>> Handle(
        GetTenantsQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetTenantsQueryResult(model));
    }
}

public sealed record GetTenantsQuery : IQuery<GetTenantsQueryResult>;

public sealed record GetTenantsQueryResult(IEnumerable<TenantM> Tenants);



