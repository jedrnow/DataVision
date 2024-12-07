using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DataVision.Domain.Entities;

namespace DataVision.Infrastructure.Data.Configurations;
public class DatabaseTableConfiguration : IEntityTypeConfiguration<DatabaseTable>
{
    public void Configure(EntityTypeBuilder<DatabaseTable> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired();
    }
}

