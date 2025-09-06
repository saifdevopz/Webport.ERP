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
        string pItemCode,
        string pItemDesc
    )
    {
        ItemM model = new()
        {
            ItemCode = pItemCode,
            ItemDesc = pItemDesc,
        };

        return model;
    }
}