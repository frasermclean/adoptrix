using System.Reflection;
using Adoptrix.Core;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public class AdoptrixDbContext(DbContextOptions<AdoptrixDbContext> options) : DbContext(options)
{
    public DbSet<Species> Species => Set<Species>();
    public DbSet<Breed> Breeds => Set<Breed>();
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
