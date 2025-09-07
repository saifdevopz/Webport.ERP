using System.ComponentModel.DataAnnotations;

namespace Webport.ERP.Common.Domain.DataTransferObjects.System;

public class UserDto
{
    public int UserId { get; set; }

    [DeniedValues(0)]
    public int TenantId { get; set; }

    [Required]
    public int RoleId { get; set; }

    [Required]
    public string? FullName { get; set; }

    [Required]
    public string? Email { get; set; }
}

public record UserWrapper<T>(T User);
public record UsersWrapper<T>(IEnumerable<T> Users);