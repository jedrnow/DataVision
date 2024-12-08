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
    }
}
