using DataVision.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataVision.Infrastructure.Data.Configurations;

public class DatabaseConfiguration : IEntityTypeConfiguration<Database>
{
    public void Configure(EntityTypeBuilder<Database> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.ConnectionString)
            .IsRequired();

        builder.HasMany(t => t.DatabaseTables)
            .WithOne(dt => dt.Database)
            .HasForeignKey(t => t.DatabaseId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(t => t.DatabaseTableColumns)
            .WithOne(dt => dt.Database)
            .HasForeignKey(t => t.DatabaseId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(t => t.DatabaseTableRows)
            .WithOne(dt => dt.Database)
            .HasForeignKey(t => t.DatabaseId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasMany(t => t.DatabaseTableCells)
            .WithOne(dt => dt.Database)
            .HasForeignKey(t => t.DatabaseId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
