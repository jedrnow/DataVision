using DataVision.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataVision.Infrastructure.Data.Configurations;
public class DatabaseTableCellConfiguration : IEntityTypeConfiguration<DatabaseTableCell>
{
    public void Configure(EntityTypeBuilder<DatabaseTableCell> builder)
    {
        builder.Property(c => c.Type)
            .IsRequired();

        builder.HasOne(c => c.DatabaseTableColumn)
            .WithMany()
            .HasForeignKey(c => c.DatabaseTableColumnId);
    }
}
