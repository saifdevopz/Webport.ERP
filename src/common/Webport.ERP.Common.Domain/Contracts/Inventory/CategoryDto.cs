using System.ComponentModel.DataAnnotations;

namespace Webport.ERP.Common.Domain.Contracts.Inventory;

public class CategoryDto
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "sssss")]
    public string CategoryCode { get; set; } = string.Empty;

    [Required]
    public string CategoryDesc { get; set; } = string.Empty;
}