﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataVision.Domain.Entities;

namespace DataVision.Infrastructure.Data.Configurations;
public class DatabaseTableConfiguration : IEntityTypeConfiguration<DatabaseTable>
{
    public void Configure(EntityTypeBuilder<DatabaseTable> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired();

        builder.HasMany(t => t.Columns)
            .WithOne(c => c.DatabaseTable)
            .HasForeignKey(c => c.DatabaseTableId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(t => t.Rows)
            .WithOne(r => r.DatabaseTable)
            .HasForeignKey(r => r.DatabaseTableId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(t => t.Cells)
            .WithOne(c => c.DatabaseTable)
            .HasForeignKey(c => c.DatabaseTableId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}

