namespace Webport.ERP.Inventory.Domain.Entities.Category;

public sealed class CategoryM : AggregateRoot, IMustHaveTenant
{

    public int TenantId { get; set; }
    public int CategoryId { get; set; }
    public required string CategoryCode { get; set; }
    public required string CategoryDesc { get; set; }

    public static CategoryM Create
    (
        string categoryCode,
        string categoryDesc
    )
    {
        CategoryM model = new()
        {
            CategoryCode = categoryCode,
            CategoryDesc = categoryDesc,
        };

        return model;
    }
}

