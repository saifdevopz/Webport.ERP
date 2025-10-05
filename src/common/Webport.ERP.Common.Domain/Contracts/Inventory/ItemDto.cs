namespace Webport.ERP.Common.Domain.Contracts.Inventory;

public class ItemDto
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemDesc { get; set; } = string.Empty;

    public CategoryDto Category { get; set; } = new();
}

public record ItemWrapper<T>(T Category);
public record ItemsWrapper<T>(IEnumerable<T> Categories);