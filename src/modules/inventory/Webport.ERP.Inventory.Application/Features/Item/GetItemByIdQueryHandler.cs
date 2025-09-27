using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Features.Item;

public class GetItemByIdQueryHandler(IInventoryRepository<ItemM> repository)
    : IQueryHandler<GetItemByIdQuery, GetItemByIdQueryResult>
{
    public async Task<Result<GetItemByIdQueryResult>> Handle(
        GetItemByIdQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.FindOneAsync(_ => _.ItemId == query.ItemId, cancellationToken);

        return model is not null
            ? Result.Success(new GetItemByIdQueryResult(model))
            : Result.Failure<GetItemByIdQueryResult>(CustomError.NotFound("Not Found", "Record not found."));
    }
}

public sealed record GetItemByIdQuery(int ItemId) : IQuery<GetItemByIdQuery>;

public sealed record GetItemByIdQueryResult(ItemM Obj);
