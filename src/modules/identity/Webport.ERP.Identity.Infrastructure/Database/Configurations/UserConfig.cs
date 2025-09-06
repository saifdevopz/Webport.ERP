using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webport.ERP.Identity.Infrastructure.Database.Configurations;

internal sealed class UserConfig : IEntityTypeConfiguration<UserM>
{
    public void Configure(EntityTypeBuilder<UserM> builder)
    {
        builder.HasKey(_ => _.UserId);

        builder.Property(_ => _.FullName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(_ => _.Email)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(_ => _.PasswordHash)
            .IsRequired();

        builder.Property(_ => _.PasswordSalt)
            .IsRequired();

        builder.HasIndex(_ => new { _.Email })
            .IsUnique();

        builder
            .HasOne(_ => _.Tenant)
            .WithMany()
            .HasForeignKey(_ => _.TenantId);

        builder
            .HasOne(_ => _.Role)
            .WithMany()
            .HasForeignKey(_ => _.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
