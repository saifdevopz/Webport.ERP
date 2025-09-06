using Webport.ERP.Identity.Application.Interfaces;

namespace Webport.ERP.Identity.Application.Features.Users;

public class GetUserByIdQueryHandler(IIdentityRepository<UserM> repository)
    : IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult>
{
    public async Task<Result<GetUserByIdQueryResult>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.UserId == query.UserId, cancellationToken);

        return model is not null
            ? Result.Success(new GetUserByIdQueryResult(model))
            : Result.Failure<GetUserByIdQueryResult>(CustomError.NotFound("Not Found", "User not found."));

    }
}

public sealed record GetUserByIdQuery(int UserId) : IQuery<GetUserByIdQueryResult>;

public sealed record GetUserByIdQueryResult(UserM User);