using System.Reflection;
using DataVision.Application.Common.Interfaces;
using DataVision.Domain.Entities;
using DataVision.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataVision.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Database> Databases => Set<Database>();
    public DbSet<DatabaseTable> DatabaseTables => Set<DatabaseTable>();
    public DbSet<DatabaseTableColumn> DatabaseTableColumns => Set<DatabaseTableColumn>();
    public DbSet<DatabaseTableRow> DatabaseTableRows => Set<DatabaseTableRow>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
