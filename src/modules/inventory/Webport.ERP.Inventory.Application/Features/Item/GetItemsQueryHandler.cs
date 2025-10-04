using Microsoft.EntityFrameworkCore;

namespace Webport.ERP.Inventory.Application.Features.Item;

public class GetItemsQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetItemsQuery, GetItemsQueryResult>
{
    public async Task<Result<GetItemsQueryResult>> Handle(
        GetItemsQuery query,
        CancellationToken cancellationToken)
    {
        List<ItemM> records = await dbContext.Items
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(new GetItemsQueryResult(records));
    }
}

public sealed record GetItemsQuery : IQuery<GetItemsQueryResult>;

public sealed record GetItemsQueryResult(IEnumerable<ItemM> Items);