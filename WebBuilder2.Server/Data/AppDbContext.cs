using Microsoft.EntityFrameworkCore;
using WebBuilder2.Server.Data.Models;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AuditableEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((AuditableEntity)entityEntry.Entity).ModifiedDateTime = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((AuditableEntity)entityEntry.Entity).CreatedDateTime = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AuditableEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((AuditableEntity)entityEntry.Entity).ModifiedDateTime = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((AuditableEntity)entityEntry.Entity).CreatedDateTime = DateTime.Now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<Site> Site { get; set; }
    public DbSet<Script> Script { get; set; }
    public DbSet<Repository> Repository { get; set; }
}
