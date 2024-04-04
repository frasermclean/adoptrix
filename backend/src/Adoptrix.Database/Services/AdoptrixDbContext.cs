using Adoptrix.Database.Configuration;
using Adoptrix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class AdoptrixDbContext(DbContextOptions<AdoptrixDbContext> options) : DbContext(options)
{
    public DbSet<Species> Species => Set<Species>();
    public DbSet<Breed> Breeds => Set<Breed>();
    public DbSet<Animal> Animals => Set<Animal>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AnimalConfiguration());
        builder.ApplyConfiguration(new BreedConfiguration());
        builder.ApplyConfiguration(new SpeciesConfiguration());
    }
}
