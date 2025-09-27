using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Features.Users;

public class GetUsersQueryHandler(IIdentityRepository<UserM> repository)
    : IQueryHandler<GetUsersQuery, GetUsersQueryResult>
{
    public async Task<Result<GetUsersQueryResult>> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetUsersQueryResult(model));
    }
}

public sealed record GetUsersQuery : IQuery<GetUsersQueryResult>;

public sealed record GetUsersQueryResult(IEnumerable<UserM> Users);