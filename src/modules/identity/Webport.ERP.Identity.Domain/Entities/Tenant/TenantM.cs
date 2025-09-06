namespace Webport.ERP.Identity.Domain.Entities.Tenant;

public sealed class TenantM : AggregateRoot
{
    public int TenantId { get; set; }
    public required string TenantName { get; set; }
    public DateTime LicenceExpiryDate { get; set; }

    public static TenantM Create(string pTenantName)
    {
        TenantM model = new()
        {
            TenantName = pTenantName,
            LicenceExpiryDate = DateTime.UtcNow.AddDays(30),
        };

        return model;
    }
}