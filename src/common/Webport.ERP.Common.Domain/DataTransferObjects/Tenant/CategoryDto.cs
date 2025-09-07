namespace Webport.ERP.Common.Domain.DataTransferObjects.Tenant;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryDesc { get; set; } = string.Empty;
}

public record CategoryWrapper<T>(T Category);
public record CategoriesWrapper<T>(IEnumerable<T> Categories);