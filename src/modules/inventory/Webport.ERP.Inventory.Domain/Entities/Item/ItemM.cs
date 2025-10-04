using Webport.ERP.Inventory.Domain.Entities.Category;

namespace Webport.ERP.Inventory.Domain.Entities.Item;

public sealed class ItemM : AggregateRoot, IMustHaveTenant
{
    public int TenantId { get; set; }
    public int ItemId { get; set; }
    public int CategoryId { get; set; }
    public CategoryM? Category { get; set; }
    public required string ItemCode { get; set; }
    public required string ItemDesc { get; set; }

    public static ItemM Create
    (
        int categoryId,
        string itemCode,
        string itemDesc
    )
    {
        ItemM model = new()
        {
            CategoryId = categoryId,
            ItemCode = itemCode,
            ItemDesc = itemDesc,
        };

        return model;
    }
}