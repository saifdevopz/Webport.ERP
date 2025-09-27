using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Features.Item;

public class GetItemsQueryHandler(IInventoryRepository<ItemM> repository)
    : IQueryHandler<GetItemsQuery, GetItemsQueryResult>
{
    public async Task<Result<GetItemsQueryResult>> Handle(
        GetItemsQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetItemsQueryResult(model));
    }
}

public sealed record GetItemsQuery : IQuery<GetItemsQueryResult>;

public sealed record GetItemsQueryResult(IEnumerable<ItemM> Obj);