using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webport.ERP.Identity.Infrastructure.Database.Configurations;

internal sealed class RolePermissionConfig : IEntityTypeConfiguration<RolePermissionM>
{
    public void Configure(EntityTypeBuilder<RolePermissionM> builder)
    {
        builder
            .HasKey(_ => new { _.RoleId, _.PermissionId });

        builder
            .HasOne<PermissionM>()
            .WithMany()
            .HasForeignKey(_ => _.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<RoleM>()
            .WithMany()
            .HasForeignKey(_ => _.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

    }

}