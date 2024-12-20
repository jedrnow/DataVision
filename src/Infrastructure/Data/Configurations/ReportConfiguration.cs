using DataVision.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataVision.Infrastructure.Data.Configurations;
public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.FileName)
            .IsRequired();

        builder.Property(t => t.Format)
            .IsRequired();
    }
}
