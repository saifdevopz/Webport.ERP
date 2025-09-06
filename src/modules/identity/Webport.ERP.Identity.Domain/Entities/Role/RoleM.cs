namespace Webport.ERP.Identity.Domain.Entities.Role;

public sealed class RoleM : AggregateRoot
{
    public int RoleId { get; }
    public required string RoleName { get; set; }
    public required string NormalizedRoleName { get; set; }

    public static RoleM Create(string roleName)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        RoleM model = new()
        {
            RoleName = roleName,
            NormalizedRoleName = roleName.ToUpperInvariant()
        };

        return model;
    }
}