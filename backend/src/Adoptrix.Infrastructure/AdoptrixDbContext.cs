﻿using Adoptrix.Domain;
using Adoptrix.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure;

public class AdoptrixDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Species> Species => Set<Species>();
    public DbSet<Breed> Breeds => Set<Breed>();
    public DbSet<Animal> Animals => Set<Animal>();

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        var connectionString = configuration.GetValue<string>("Database:ConnectionString");

        builder.LogTo(Console.WriteLine, LogLevel.Information);
        builder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AnimalConfiguration());
        builder.ApplyConfiguration(new BreedConfiguration());
        builder.ApplyConfiguration(new SpeciesConfiguration());
    }
}