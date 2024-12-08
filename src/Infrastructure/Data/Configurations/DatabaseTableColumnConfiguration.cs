using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataVision.Domain.Entities;

namespace DataVision.Infrastructure.Data.Configurations;
public class DatabaseTableColumnConfiguration : IEntityTypeConfiguration<DatabaseTableColumn>
{
    public void Configure(EntityTypeBuilder<DatabaseTableColumn> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired();

        builder.Property(t => t.Type)
            .IsRequired();

        builder.HasMany(t => t.Cells)
            .WithOne(c => c.DatabaseTableColumn)
            .HasForeignKey(c =>  c.DatabaseTableColumnId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
