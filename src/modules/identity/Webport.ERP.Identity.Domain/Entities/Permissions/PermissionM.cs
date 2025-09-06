namespace Webport.ERP.Identity.Domain.Entities.Permissions;

public sealed class PermissionM : AggregateRoot
{
    public int PermissionId { get; }
    public string PermissionCode { get; private set; } = string.Empty;

    public static PermissionM Create(string permissionCode)
    {
        PermissionM model = new()
        {
            PermissionCode = permissionCode,
        };

        return model;
    }
}