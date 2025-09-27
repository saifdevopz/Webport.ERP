using Webport.ERP.Inventory.Application.Interfaces;

namespace Webport.ERP.Inventory.Application.Features.Category;

public class GetCategoriesQueryHandler(IInventoryRepository<CategoryM> repository)
    : IQueryHandler<GetCategoriesQuery, GetCategoriesQueryResult>
{
    public async Task<Result<GetCategoriesQueryResult>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var model = await repository.GetAllAsync(cancellationToken);

        return Result.Success(new GetCategoriesQueryResult(model));
    }
}

public sealed record GetCategoriesQuery : IQuery<GetCategoriesQueryResult>;

public sealed record GetCategoriesQueryResult(IEnumerable<CategoryM> Categories);