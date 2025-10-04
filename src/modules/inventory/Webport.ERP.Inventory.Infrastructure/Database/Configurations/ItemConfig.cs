﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Webport.ERP.Inventory.Domain.Entities.Item;

namespace Webport.ERP.Inventory.Infrastructure.Database.Configurations;

public sealed class ItemConfig : IEntityTypeConfiguration<ItemM>
{
    public void Configure(EntityTypeBuilder<ItemM> builder)
    {
        builder.HasKey(_ => _.ItemId);

        builder.HasOne(i => i.Category)
            .WithMany()
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(_ => new { _.TenantId, _.ItemCode })
            .IsUnique();

        builder.Property(_ => _.ItemCode)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(_ => _.ItemDesc)
               .IsRequired()
               .HasMaxLength(50);
    }
}