using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webport.ERP.Identity.Infrastructure.Database.Configurations;

internal sealed class TenantConfig : IEntityTypeConfiguration<TenantM>
{
    public void Configure(EntityTypeBuilder<TenantM> builder)
    {
        builder
            .HasKey(_ => _.TenantId);

        builder.Property(_ => _.TenantName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(_ => _.LicenceExpiryDate)
            .IsRequired();

        builder.HasIndex(_ => new { _.TenantName })
            .IsUnique();
    }
}
