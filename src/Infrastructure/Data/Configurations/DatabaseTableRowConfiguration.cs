using DataVision.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataVision.Infrastructure.Data.Configurations;
public class DatabaseTableRowConfiguration : IEntityTypeConfiguration<DatabaseTableRow>
{
    public void Configure(EntityTypeBuilder<DatabaseTableRow> builder)
    {

    }
}
