namespace Webport.ERP.Inventory.Domain.Entities.Category;

public sealed class CategoryM : AggregateRoot, IMustHaveTenant
{
    public int TenantId { get; set; }
    public int CategoryId { get; set; }
    public required string CategoryCode { get; set; }
    public required string CategoryDesc { get; set; }    

    public static CategoryM Create
    (
        string pCategoryCode,
        string pCategoryDesc
    )
    {
        CategoryM model = new()
        {
            CategoryCode = pCategoryCode,
            CategoryDesc = pCategoryDesc,
        };

        return model;
    }
}

