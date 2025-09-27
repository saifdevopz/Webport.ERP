using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Features.Tenant;

public class GetTenantByIdQueryHandler(IIdentityRepository<TenantM> repository)
    : IQueryHandler<GetTenantByIdQuery, GetTenantByIdQueryResult>
{
    public async Task<Result<GetTenantByIdQueryResult>> Handle(
        GetTenantByIdQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.TenantId == query.TenantId, cancellationToken);

        return model is not null
            ? Result.Success(new GetTenantByIdQueryResult(model))
            : Result.Failure<GetTenantByIdQueryResult>(CustomError.NotFound("Not Found", "Tenant not found."));
    }
}

public sealed record GetTenantByIdQuery(int TenantId) : IQuery<GetTenantByIdQueryResult>;

public sealed record GetTenantByIdQueryResult(TenantM Tenant);